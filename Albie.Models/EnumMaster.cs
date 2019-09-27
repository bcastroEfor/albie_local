using System.ComponentModel;

namespace Albie.Models
{
    public class EnumMaster
    {
        public enum Estado_Enum
        {
            [Description("Pendiente")] [DefaultValue("0")] Pendiente = 0,
            [Description("Confirmado")] [DefaultValue("1")] Confirmado = 1,
            [Description("Creado BC")] [DefaultValue("2")] CreadoBC = 2,
            [Description("Error BC")] [DefaultValue("3")] ErrorBC = 3,
            [Description("Anulado")] [DefaultValue("4")] Anulado = 4,
            [Description("Registrado")] [DefaultValue("5")] Registrado = 5
        }
    }
}
