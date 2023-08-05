using Microsoft.AspNetCore.Mvc;

using TallerMecanicoCore.Models;
using Microsoft.EntityFrameworkCore;

using TallerMecanicoCore.DTO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

using iTextSharp.text;
using iTextSharp.text.pdf;


namespace TallerMecanicoCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly TallerContext _context;

        public HomeController(TallerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var datosPresupuesto = _context.DatosPresupuesto
                .FromSqlRaw("EXECUTE ObtenerDatosPresupuesto @id = null")
                .ToList();


            var viewModelList = datosPresupuesto.Select(dp => new DatosPresupuesto
            {
                Id = dp.Id,
                Nombre = dp.Nombre,
                Apellido = dp.Apellido,
                Email = dp.Email,
                Total = dp.Total,
                Marca = dp.Marca,
                Modelo = dp.Modelo,
                Patente = dp.Patente,
                CantidadPuertas = dp.CantidadPuertas,
                Cilindrada = dp.Cilindrada,
                Descripcion = dp.Descripcion,
                Manodeobra = dp.Manodeobra,
                Tiempo = dp.Tiempo,
                NombreRepuesto = dp.NombreRepuesto,
                Precio = dp.Precio
            }).ToList();

            return View(viewModelList);
        }
        [HttpGet]
        public async Task<IActionResult> GenerarPresupuesto(int? id)
        {
            var datosPresupuesto = await _context.DatosPresupuesto
                .FromSqlInterpolated($"EXECUTE ObtenerDatosPresupuesto {id}")
                .ToListAsync();


            var document = new Document();
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();


            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);


            var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

            foreach (var item in datosPresupuesto)
            {

                document.Add(new Paragraph("Nombre: ", titleFont));
                document.Add(new Paragraph(item.Nombre, dataFont));


                document.Add(new Paragraph("Apellido: ", titleFont));
                document.Add(new Paragraph(item.Apellido, dataFont));


                document.Add(new Paragraph("Email: ", titleFont));
                document.Add(new Paragraph(item.Email, dataFont));


                document.Add(new Paragraph("Marca: ", titleFont));
                document.Add(new Paragraph(item.Marca, dataFont));

                document.Add(new Paragraph("Modelo: ", titleFont));
                document.Add(new Paragraph(item.Modelo, dataFont));

                if(item.Cilindrada != null)
                {
                    document.Add(new Paragraph("Cilindrada: ", titleFont));
                    document.Add(new Paragraph(item.Cilindrada.ToString(), dataFont));
                }
                else
                {
                    document.Add(new Paragraph("Cantidad de puertas: ", titleFont));
                    document.Add(new Paragraph(item.CantidadPuertas.ToString(), new Font())); 
                }

                document.Add(new Paragraph("Patente: ", titleFont));
                document.Add(new Paragraph(item.Patente, dataFont));

                document.Add(new Paragraph("Desperfecto: ", titleFont));
                document.Add(new Paragraph(item.Descripcion, dataFont));

                document.Add(new Paragraph("Repuestos: ", titleFont));
                document.Add(new Paragraph(item.NombreRepuesto, dataFont));

                document.Add(new Paragraph("Precio x repuesto: ", titleFont));
                document.Add(new Paragraph(item.Precio.ToString(), dataFont));

                document.Add(new Paragraph("Tiempo de reparacion en dias: ", titleFont));
                document.Add(new Paragraph(item.Tiempo.ToString(), dataFont));

                document.Add(new Paragraph("Total: ", titleFont));
                document.Add(new Paragraph(item.Total.ToString(), dataFont));

                document.Add(Chunk.NEWLINE);
            }

            document.Close();
            return File(memoryStream.ToArray(), "application/pdf", "presupuesto.pdf");
        }



        public IActionResult CreatePresupuesto()
        {

            return View("Presupuesto");
        }

        [HttpPost]
        public IActionResult GenerarPresupuesto(Params parametros)
        {

            List<int> ids = parametros.Repuesto.Select(int.Parse).ToList();

            var PreciosXRepuesto = _context.Repuestos
                .Where(a => ids.Contains(a.Id))
                .Select(a => new { Codigo = a.Id, Precio = a.Precio })
                .ToList();
            var totalPrecio = PreciosXRepuesto.Sum(r => r.Precio);

            var totalConManoDeObra = (parametros.TiempoReparacion * 130) + ( parametros.TiempoReparacion *100/10) + parametros.ManoObra + totalPrecio;

            parametros.Total = totalConManoDeObra;

            try
            {
                string parametrosXml = ConvertParamsToXml(parametros);
                _context.Database.ExecuteSqlRaw("EXEC InsertarPresupuesto @ParametrosXML", new SqlParameter("ParametrosXML", parametrosXml));

                return Ok("El presupuesto se ha generado exitosamente");
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido generar el presupuesto correspondiente");
            }
        }

        public IActionResult EliminarPresupuesto(int presupuestoId)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("EXEC EliminarPresupuesto @PresupuestoId", new SqlParameter("PresupuestoId", presupuestoId));
                return Ok("El presupuesto se ha eliminado exitosamente");
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido eliminar el presupuesto correspondiente");
            }
        }



        #region xmlConvert
        private string ConvertParamsToXml(Params parametros)
            {
                XDocument xmlDocument = new XDocument(
             new XElement("Params",
            new XElement("TipoVehiculo", parametros.TipoVehiculo),
            new XElement("CantidadPuertas", parametros.CantidadPuertas),
            new XElement("TipoAuto", parametros.TipoAuto),
            new XElement("Cilindrada", parametros.Cilindrada),
            new XElement("Marca", parametros.Marca),
            new XElement("Modelo", parametros.Modelo),
            new XElement("Patente", parametros.Patente),
            new XElement("Nombre", parametros.Nombre),
            new XElement("Apellido", parametros.Apellido),
            new XElement("Email", parametros.Email),
            new XElement("Total", parametros.Total),
            new XElement("ManoObra", parametros.ManoObra),
            new XElement("TiempoReparacion", parametros.TiempoReparacion)
            )
         );

            if (parametros.Desperfecto != null && parametros.Desperfecto.Any())
            {
                xmlDocument.Root.Add(
                new XElement("Desperfecto",
                parametros.Desperfecto.Select(d => new XElement("Descripcion", d)))
                );
            }

    

             if (parametros.Repuesto != null && parametros.Repuesto.Any())
             {
                xmlDocument.Root.Add(
                 new XElement("Repuesto",
                parametros.Repuesto.Select(r => new XElement("Repuesto", r)))
                );
              }

                return xmlDocument.ToString();
        }
        #endregion

        #region Helpers
        [HttpGet]
        public IActionResult Repuestos()
        {
            var repuestos = _context.Repuestos
                                    .Select(a => new { Codigo = a.Id, Descripcion = a.Nombre })
                                    .ToList();

            return Ok(repuestos);
        }
        #endregion
    }
}