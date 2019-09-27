namespace ActioBP.Identity.ModelsJS
{
    public class Roles
    {
        public const string Centro = "centro"; //CRUD sobre centros permitidos
        public const string Campus = "centro"; // CRUD sobre campus permitidos
        public const string Trabajador = "trabajador"; // Tiene acceso como trabajador a todas las funcionalidades
        public const string Profesor = "profesor"; // Tiene acceso a todas las funcionalidades de Profesor Permitido
        public const string Inscripcion = "inscripcion"; // Tiene acceso a todas las funcionalidades de inscripcion
        public const string Factura = "factura"; //	Tiene acceso a todas las funcionalidades de Factura Permitidas
        // public const string MovimientosPago -> Iría con facturación? Entiendo que si. // Tiene acceso a todas las funcionalidades de movimientos de pago permitidos
        public const string AdministracionCole = "admincole";//Tiene acceso a todas las funcionalidades de administración de cole permitidos
        public const string Transfer = "transfer"; // Tiene acceso a todas las funcionalidades de transfer permitidos
        public const string Alojamiento = "alojamiento"; // Tiene acceso a todas las funcionalidades de transfer permitidos
        public const string Admin = "admin"; //	Tendrá acceso a toda la información. Cuando se aplica, se le meten todos los roles por defecto.
        public const string AdminUser = "adminuser"; // puede modificar y dar de altas logins y accesos.

    }
}
