using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Parte1;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace Parte3.Controllers
{

    [ApiController]
    [Route("api")]
    public class SDESController : Controller
    {
        [Route("cipher")]
        [HttpPost]
        public ActionResult Cipher(string key, IFormFile file, [FromRoute] string name)
        {
            try
            {
                int keynum = Convert.ToInt32(key);
                if (file.Length > 0 && keynum < 1024)
                {
                    List<byte> final = new List<byte>();
                    const int MAX_BUFFER = 1000;
                    byte[] buffer = new byte[MAX_BUFFER];
                    int bytesRead;
                    var fileBytes = (dynamic)null;
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    //PERMUTACIONES
                    StreamReader lector = new StreamReader("Permutaciones.txt");
                    int[] p8 = new int[10];
                    new Permutaciones().llenarpermutaciones(p8, lector.ReadLine());
                    int[] p10 = new int[10];
                    new Permutaciones().llenarpermutaciones(p10, lector.ReadLine());
                    int[] p4 = new int[10];
                    new Permutaciones().llenarpermutaciones(p4, lector.ReadLine());
                    int[] ep = new int[10];
                    new Permutaciones().llenarpermutaciones(ep, lector.ReadLine());
                    int[] ip = new int[10];
                    new Permutaciones().llenarpermutaciones(ip, lector.ReadLine());
                    int[] ip1 = new int[10];
                    new Permutaciones().llenarpermutaciones(ip1, lector.ReadLine());
                    using (FileStream fs = File(fileBytes, file.ContentType))
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        while ((bytesRead = bs.Read(buffer, 0, MAX_BUFFER)) != 0) //leyendo 1000 bytes a la vez
                        {
                            final.Clear();
                            foreach (var b in buffer)
                            {
                                SDES sdes = new ImplementationClass();
                                string k = Convert.ToString(keynum, 2).PadLeft(10, '0');
                                var keys = sdes.generateKey(k, p10, p8);
                                final.Add(sdes.Enconde(k, keys.key1, keys.key2, p4, ep, ip, ip1));
                            }
                            string path = Environment.CurrentDirectory + "\\" + name + ".txt"; //extension
                            using (var stream = new FileStream(path, FileMode.Append))
                            {
                                stream.Write(final.ToArray(), 0, final.Count);
                            }
                        }
                    }
                    return Ok("Archivo cifrado en: " + Environment.CurrentDirectory);
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
        public ActionResult Decipher(string key, IFormFile file, [FromRoute] string name)
        {
            try
            {
                int keynum = Convert.ToInt32(key);
                if (file.Length > 0 && keynum < 1024)
                {
                    List<byte> final = new List<byte>();
                    const int MAX_BUFFER = 1000;
                    byte[] buffer = new byte[MAX_BUFFER];
                    int bytesRead;
                    var fileBytes = (dynamic)null;
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    //PERMUTACIONES
                    StreamReader lector = new StreamReader("Permutaciones.txt");
                    int[] p8 = new int[10];
                    new Permutaciones().llenarpermutaciones(p8, lector.ReadLine());
                    int[] p10 = new int[10];
                    new Permutaciones().llenarpermutaciones(p10, lector.ReadLine());
                    int[] p4 = new int[10];
                    new Permutaciones().llenarpermutaciones(p4, lector.ReadLine());
                    int[] ep = new int[10];
                    new Permutaciones().llenarpermutaciones(ep, lector.ReadLine());
                    int[] ip = new int[10];
                    new Permutaciones().llenarpermutaciones(ip, lector.ReadLine());
                    int[] ip1 = new int[10];
                    new Permutaciones().llenarpermutaciones(ip1, lector.ReadLine());
                    using (FileStream fs = File(fileBytes, file.ContentType))
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        while ((bytesRead = bs.Read(buffer, 0, MAX_BUFFER)) != 0) //leyendo 1000 bytes a la vez
                        {
                            final.Clear();
                            foreach (var b in buffer)
                            {
                                SDES sdes = new ImplementationClass();
                                string k = Convert.ToString(keynum, 2).PadLeft(10, '0');
                                var keys = sdes.generateKey(k, p10, p8);
                                final.Add(sdes.Enconde(k, keys.key2, keys.key1, p4, ep, ip, ip1));
                            }
                            string path = Environment.CurrentDirectory + "\\" + name + ".txt"; //extension
                            using (var stream = new FileStream(path, FileMode.Append))
                            {
                                stream.Write(final.ToArray(), 0, final.Count);
                            }
                        }
                    }
                    return Ok("Archivo cifrado en: " + Environment.CurrentDirectory);
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


