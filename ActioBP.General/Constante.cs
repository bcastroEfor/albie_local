namespace ActioBP.General
{
    public static class Constante
    {

        public struct Pattern
        {
            public const string Email = "^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
            public const string TelefonoEspana = "([0-9]{9})";
            public const int TelefonoEspana_Length = 9;
            public const string TelefonoUSA = "([0-9]{3}) [0-9]{3}-[0-9]{4}";
            public const int TelefonoUSA_Length = 11;
        }

        public struct MaxLength
        {
            public const int NIF_CIF = 30;
            public const int DNI = 30;
            public const int Passport = 30;
            public const int Nombre = 100;
            public const int Apellidos = 200;
            public const int CodigoPostal = 10;
            public const int Provincia = 10;
            public const int Ciudad = 50;
            public const int Direcccion = 100;
            public const int PersonaContacto = 100;
            public const int Email = 50;
            public const int Telefono = 20;


            public const int pst_Nombre = 100;
            public const int pst_Nombre_Url = 100;
            public const int pst_Titulo = 90;
            public const int pst_MetaDescription = 150;
            public const int pst_MetaKeyWords = 150;
            public const int pst_MetaKeyWords_Sobrante = 50;
            public const int pst_Description = 250;

            public const string txt_Preview_Sigue = "...";
            public const int txt_Preview_Caracteres = 20;
        }
    }
}
