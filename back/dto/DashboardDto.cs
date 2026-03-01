namespace back.dto
{
    public class DashboardDto
    {
        public int CitasHoy { get; set; }
        public int CitasSemana { get; set; }
        public int ClientesActivos { get; set; }
        public int CanceladasMes { get; set; }
        public List<ReservaMiniDto> ProximasCitas { get; set; } = new();
        public List<TopServicioDto> TopServicios { get; set; } = new();
    }

    public class ReservaMiniDto
    {
        public int Id_Reserva { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora_Inicio { get; set; }
        public string? Estado { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ServicioNombre { get; set; }
        public string? EmpleadoEspecialidad { get; set; }
    }

    public class TopServicioDto
    {
        public int Id_Servicio { get; set; }
        public string? Nombre { get; set; }
        public int Total { get; set; }
    }
}