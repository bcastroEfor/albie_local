using System.Collections.Generic;
using ActioBP.General;

namespace ActioBP.Mvc.Complement
{
    public static class Metas 
    {
        public static void SetMetasConfig(ref string MetaKeyWords, ref  string Title, ref string MetaDescription, ref  string Descripcion_Corta, string Nombre, string Descripcion_Larga, ICollection<string> tagList)
        {
            SetMetaDescription(ref MetaDescription, Descripcion_Corta, Descripcion_Larga);
            SetMetaKeyWords(ref MetaKeyWords, tagList);
            SetMetaTitle(ref Title, Nombre);
            SetDescriptionCorta(ref Descripcion_Corta,Descripcion_Larga);
        }

        public static string SetMetaDescription(ref string MetaDescription, string Descripcion_Corta, string Descripcion_Larga)
        {
            if (string.IsNullOrEmpty(MetaDescription))
                if (!string.IsNullOrEmpty(Descripcion_Corta))
                {
                    string desc = TextUtility.ClearHtml(Descripcion_Corta);
                    if (desc.Length > Constante.MaxLength.pst_MetaDescription)
                        MetaDescription = desc.Substring(0, Constante.MaxLength.pst_MetaDescription);
                    else
                        MetaDescription = desc;
                }
                else if (!string.IsNullOrEmpty(Descripcion_Larga))
                {
                    string desc = TextUtility.ClearHtml(Descripcion_Larga);
                    if (desc.Length > Constante.MaxLength.pst_MetaDescription)
                        MetaDescription = desc.Substring(0, Constante.MaxLength.pst_MetaDescription);
                    else
                        MetaDescription = desc;
                }
            return MetaDescription;
        }

        public static string SetMetaKeyWords(ref string MetaKeyWords, ICollection<string> tagList)
        {
            if (MetaKeyWords.Length<Constante.MaxLength.pst_MetaKeyWords && tagList!=null)
            {
                foreach (var meta in tagList)
                {
                    if (MetaKeyWords.Length + meta.Length > Constante.MaxLength.pst_MetaKeyWords) break;
                    else MetaKeyWords += "," + meta;
                }
            }
            return MetaKeyWords;
        }

        public static string SetMetaKeyWords(ref string MetaKeyWords, string metaKeyWordsExtra)
        {
            if (MetaKeyWords.Length < Constante.MaxLength.pst_MetaKeyWords)
            {
                foreach (string meta in metaKeyWordsExtra.Split(','))
                {
                    if (MetaKeyWords.Length + meta.Length > Constante.MaxLength.pst_MetaKeyWords) break;
                    else MetaKeyWords+=","+meta;
                }
            }
            return MetaKeyWords;
        }

        public static string SetMetaTitle(ref string Titulo, string Nombre)
        {
            if (string.IsNullOrEmpty(Titulo))
            {
                Titulo = Nombre;
            }
            return Titulo;
        }

        public static string SetDescriptionCorta(ref string Descripcion_Corta, string Descripcion_Larga)
        {
            if (string.IsNullOrEmpty(Descripcion_Corta))
            {
                if (Descripcion_Larga.Length > Constante.MaxLength.pst_Description)
                    Descripcion_Corta = Descripcion_Larga.Substring(0, Constante.MaxLength.pst_Description);
                else
                    Descripcion_Corta = Descripcion_Larga;

            }
            return Descripcion_Corta;
        }
    }
}
