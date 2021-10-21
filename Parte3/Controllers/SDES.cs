using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parte1;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Data.OleDb;
using Microsoft.AspNetCore.Hosting;

namespace Parte3.Controllers
{

    [ApiController]
    [Route("api")]
    public class SDESController : Controller
    {
        public static IWebHostEnvironment Environment;
        public SDESController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }

        [Route("cipher/{name}")]
        [HttpPost]
        public ActionResult Cipher([FromRoute] string name, string key, IFormFile file)
        {
            try
            {
                int keynum = Convert.ToInt32(key);
                if (file.Length > 0 && keynum < 1024)
                {
                    List<byte> final = new List<byte>();
                    //PERMUTACIONES
                    StreamReader lector = new StreamReader("Permutations.txt");
                    int[] p10 = new int[10];
                    new Permutaciones().llenarpermutaciones(p10, lector.ReadLine());
                    int[] p8 = new int[8];
                    new Permutaciones().llenarpermutaciones(p8, lector.ReadLine());
                    int[] p4 = new int[4];
                    new Permutaciones().llenarpermutaciones(p4, lector.ReadLine());
                    int[] ep = new int[8];
                    new Permutaciones().llenarpermutaciones(ep, lector.ReadLine());
                    int[] ip = new int[8];
                    new Permutaciones().llenarpermutaciones(ip, lector.ReadLine());
                    int[] ip1 = new int[8];
                    new Permutaciones().llenarpermutaciones(ip1, lector.ReadLine());
                    //ARCHIVO WEB A UPLOADS
                    if (!Directory.Exists(Environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(Environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream stream = new FileStream(Environment.WebRootPath + "\\Upload\\" + file.FileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //LECTURA SIN BUFFER
                    byte[] x = System.IO.File.ReadAllBytes(Environment.WebRootPath + "\\Upload\\" + file.FileName);
                    foreach (byte b in x)
                    {
                        if (b != 0)
                        {
                            SDES sdes = new ImplementationClass();
                            string k = Convert.ToString(keynum, 2).PadLeft(10, '0');
                            var keys = sdes.generateKey(k, p10, p8);
                            final.Add(sdes.Enconde(Convert.ToString(b, 2).PadLeft(8, '0'), keys.key1, keys.key2, p4, ep, ip, ip1));
                        }
                    }
                    //ESCRITURA DE ARCHIVO
                    System.IO.File.WriteAllBytes(name+ ".sdes", final.ToArray());
                    return Ok("Archivo cifrado en: " + System.Environment.CurrentDirectory);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        [Route("decipher")]
        [HttpPost]
        public ActionResult Decipher(string key, IFormFile file)
        {
            try
            {
                int keynum = Convert.ToInt32(key);
                if (file.Length > 0 && keynum < 1024)
                {
                    List<byte> final = new List<byte>();
                    //PERMUTACIONES
                    StreamReader lector = new StreamReader("Permutations.txt");
                    int[] p10 = new int[10];
                    new Permutaciones().llenarpermutaciones(p10, lector.ReadLine());
                    int[] p8 = new int[8];
                    new Permutaciones().llenarpermutaciones(p8, lector.ReadLine());
                    int[] p4 = new int[4];
                    new Permutaciones().llenarpermutaciones(p4, lector.ReadLine());
                    int[] ep = new int[8];
                    new Permutaciones().llenarpermutaciones(ep, lector.ReadLine());
                    int[] ip = new int[8];
                    new Permutaciones().llenarpermutaciones(ip, lector.ReadLine());
                    int[] ip1 = new int[8];
                    new Permutaciones().llenarpermutaciones(ip1, lector.ReadLine());
                    //ARCHIVO WEB A UPLOADS
                    if (!Directory.Exists(Environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(Environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream stream = new FileStream(Environment.WebRootPath + "\\Upload\\" + file.FileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    //LECTURA SIN BUFFER
                    byte[] x = System.IO.File.ReadAllBytes(Environment.WebRootPath + "\\Upload\\" + file.FileName);
                    foreach (byte b in x)
                    {
                        if (b != 0)
                        {
                            SDES sdes = new ImplementationClass();
                            string k = Convert.ToString(keynum, 2).PadLeft(10, '0');
                            var keys = sdes.generateKey(k, p10, p8);
                            final.Add(sdes.Enconde(Convert.ToString(b, 2).PadLeft(8, '0'), keys.key2, keys.key1, p4, ep, ip, ip1));
                        }
                    }
                    //ARREGLAR NOMBRE
                    int found = file.FileName.IndexOf(".sdes");
                    string pathcipher = System.Environment.CurrentDirectory +"\\"+ file.FileName.Substring(0, found) + "_Descifrado" + ".txt"; //extension
                    System.IO.File.WriteAllBytes(pathcipher, final.ToArray());
                    return Ok("Archivo cifrado en: " + System.Environment.CurrentDirectory);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

    }

    public class Permutaciones
    {
        public void llenarpermutaciones(int[] permutacion, string linea)
        {
            var x = linea.Split(',');
            for (int i = 0; i < x.Length; i++)
            {
                permutacion[i] = Convert.ToInt32(x[i]);
            }
        }
    }

}


