using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ActioBP.Security
{
    public class Sec
    {
        public static string JwtSecurityKey = "sody0mv7m4f6ewcne1wsnf";
        // Crea la llave y el vector de inicialización para la
        // contraseña de protección del contenido a
        // encriptar o desencriptar usando un algoritmo basado
        // en TripleDES:
        public static TripleDES CrearDES(string clave)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(clave));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        // Encripta una cadena de caracteres usando una contraseña personalizada:
        public static string EncriptarCadenaDeCaracteres(string textoPlano)
        {
            // Primero debemos convertir el texto plano en `textoPlano`
            // en un arreglo de bytes:
            byte[] textoPlanoBytes = Encoding.Unicode.GetBytes(textoPlano);

            // Uso de un flujo de memoria para la contención de los bytes:
            MemoryStream flujoMemoria = new MemoryStream();

            // Creación de la clave de protección y el vector de inicialización:
            TripleDES des = CrearDES(GetClave());

            // Creación del codificador para la escritura al flujo de memoria:
            CryptoStream flujoEncriptacion = new CryptoStream(flujoMemoria, des.CreateEncryptor(), CryptoStreamMode.Write);

            // Escritura del arreglo de bytes sobre el flujo de memoria:
            flujoEncriptacion.Write(textoPlanoBytes, 0, textoPlanoBytes.Length);
            flujoEncriptacion.FlushFinalBlock();

            // Retorna representación legible de la cadena encriptada:
            return Convert.ToBase64String(flujoMemoria.ToArray());
        }

        // Descripta una cadena encriptada usando una contraseña de protección:
        public static string DesencriptarCadenaDeCaracteres(string textoEncriptado)
        {
            // Primero debemos convertir el texto plano en `textoPlano`
            // en un arreglo de bytes:
            byte[] bytesEncriptados = Convert.FromBase64String(textoEncriptado);

            // Uso de un flujo de memoria para la contención de los bytes:
            MemoryStream flujoMemoria = new MemoryStream();

            // Creación de la clave de protección y el vector de inicialización:
            TripleDES des = CrearDES(GetClave());

            // Creación de decodificador:
            CryptoStream flujoDesencriptacion = new CryptoStream(flujoMemoria, des.CreateDecryptor(), CryptoStreamMode.Write);

            // Escritura del arreglo de bytes sobre el flujo de memoria:
            flujoDesencriptacion.Write(bytesEncriptados, 0, bytesEncriptados.Length);
            flujoDesencriptacion.FlushFinalBlock();

            // Conversión del flujo de datos en una cadena de caracteres:
            return Encoding.Unicode.GetString(flujoMemoria.ToArray());
        }

        private static string GetClave()
        {
            return "seqi0wGbG7MmJdHiQdX6QqKyh3UAImpr";
        }


    }
}

