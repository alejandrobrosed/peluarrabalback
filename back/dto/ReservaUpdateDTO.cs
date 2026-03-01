namespace back.dto
{
    


public class ReservaUpdateDTO
{
    public int Id_Cliente { get; set; }
    public int Id_Servicio { get; set; }
    public int Id_Empleado { get; set; }
    public DateOnly Fecha { get; set; }
    public string? Estado { get; set; }
    public TimeOnly Hora_Inicio { get; set; }
    public string? Observaciones { get; set; }
}
}