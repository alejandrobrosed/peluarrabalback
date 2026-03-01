-- CREATE DATABASE peluqueria;
-- USE peluqueria;

CREATE TABLE usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    telefono VARCHAR(20),
    password VARCHAR(255) NOT NULL,
    rol ENUM("cliente", "admin", "empleado") NOT NULL,
    fecha_registro DATE DEFAULT CURRENT_TIMESTAMP,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE servicios (
    id_servicio INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    descripcion TEXT,
    duracion_minutos INT NOT NULL,
    precio DECIMAL(6,2) NOT NULL,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE productos (
    id_producto INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    marca VARCHAR(100),
    descripcion TEXT,
    precio_venta DECIMAL(6,2) NOT NULL,
    stock INT DEFAULT 0,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE empleados (
    id_empleado INT AUTO_INCREMENT PRIMARY KEY,
    id_usuario INT NOT NULL,
    especialidad VARCHAR(100),
    FOREIGN KEY(id_usuario) REFERENCES usuarios(id_usuario)
);

CREATE TABLE horarios (
    id_horario INT AUTO_INCREMENT PRIMARY KEY,
    id_empleado INT NOT NULL,
    dia_semana ENUM ("Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado") NOT NULL,
    hora_inicio TIME NOT NULL,
    hora_fin TIME NOT NULL,
    FOREIGN KEY(id_empleado) REFERENCES empleados(id_empleado)
);

CREATE TABLE reservas (
    id_reserva INT AUTO_INCREMENT PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_servicio INT NOT NULL,
    id_empleado INT NOT NULL,
    fecha DATE NOT NULL,
    hora_inicio TIME NOT NULL,
    estado ENUM ("pendiente", "confirmada", "cancelada", "completada") DEFAULT "pendiente",
    observaciones TEXT,
    FOREIGN KEY(id_cliente) REFERENCES usuarios(id_usuario),
    FOREIGN KEY(id_servicio) REFERENCES servicios(id_servicio),
    FOREIGN KEY(id_empleado) REFERENCES empleados(id_empleado)
);

CREATE TABLE ventas (
    id_venta INT AUTO_INCREMENT PRIMARY KEY,
    id_cliente INT NOT NULL,
    fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
    total DECIMAL(8,2) NOT NULL,
    FOREIGN KEY(id_cliente) REFERENCES usuarios(id_usuario)
);

CREATE TABLE ventas_detalles (
    id_detalle INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(6,2) NOT NULL,
    FOREIGN KEY(id_venta) REFERENCES ventas(id_venta),
    FOREIGN KEY(id_producto) REFERENCES productos(id_producto)
);
´


INSERT INTO usuarios (nombre, apellidos, email, telefono, password, rol) VALUES
('Admin', 'Principal', 'admin@arrabal.com', '600000001', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'admin'),


('Laura', 'García', 'laura@arrabal.com', '600000002', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'empleado'),
('Carlos', 'López', 'carlos@arrabal.com', '600000003', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'empleado'),
('Marta', 'Ruiz', 'marta@arrabal.com', '600000004', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'empleado'),
('Ana', 'Fernández', 'ana@arrabal.com', '600000005', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'empleado'),
('David', 'Moreno', 'david@arrabal.com', '600000006', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'empleado'),
('Lucía', 'Torres', 'lucia@arrabal.com', '600000007', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'empleado'),


('Juan', 'Pérez', 'juan@gmail.com', '611111111', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'cliente'),
('María', 'Gómez', 'maria@gmail.com', '622222222', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'cliente'),
('Pedro', 'Sánchez', 'pedro@gmail.com', '633333333', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'cliente'),
('Elena', 'Martín', 'elena@gmail.com', '644444444', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'cliente'),
('Luis', 'Rodríguez', 'luis@gmail.com', '655555555', '$2a$11$wGaFmMxH..BSbpKn6448teKX8/XKb4m2r6yl.9BpUae589ExLj94y', 'cliente');


INSERT INTO empleados (id_usuario, especialidad) VALUES
(2, 'Corte y Styling'),
(3, 'Barbería y Afeitado'),
(4, 'Coloración y Mechas'),
(5, 'Tratamientos Capilares'),
(6, 'Peinados y Eventos'),
(7, 'Asesoría de Imagen');


INSERT INTO servicios (nombre, descripcion, duracion_minutos, precio) VALUES
('Corte Caballero', 'Corte masculino moderno o clásico', 30, 18.00),
('Corte Mujer', 'Corte personalizado femenino', 45, 25.00),
('Barba', 'Arreglo y perfilado de barba', 20, 12.00),
('Color', 'Coloración completa o mechas', 90, 35.00),
('Tratamientos', 'Tratamientos capilares de hidratación y reparación', 60, 30.00),
('Peinado', 'Peinados para eventos y ocasiones especiales', 40, 20.00);


INSERT INTO productos (nombre, marca, descripcion, precio_venta, stock) VALUES
('Champú Reparador', 'LOréal', 'Champú para cabello dañado', 12.50, 20),
('Mascarilla Hidratante', 'Kérastase', 'Hidratación profunda', 18.90, 15),
('Cera Moldeadora', 'American Crew', 'Fijación flexible', 9.95, 30),
('Aceite Capilar', 'Moroccanoil', 'Brillo y nutrición', 22.00, 10),
('Espuma Volumen', 'Schwarzkopf', 'Aporta volumen y fijación', 11.75, 25);

INSERT INTO horarios (id_empleado, dia_semana, hora_inicio, hora_fin) VALUES
(1, 'Lunes', '09:00:00', '14:00:00'),
(1, 'Lunes', '16:00:00', '20:00:00'),
(2, 'Martes', '09:00:00', '14:00:00'),
(2, 'Martes', '16:00:00', '20:00:00'),
(3, 'Miércoles', '09:00:00', '14:00:00'),
(3, 'Miércoles', '16:00:00', '20:00:00'),
(4, 'Jueves', '09:00:00', '14:00:00'),
(4, 'Jueves', '16:00:00', '20:00:00'),
(5, 'Viernes', '09:00:00', '14:00:00'),
(5, 'Viernes', '16:00:00', '20:00:00'),
(6, 'Sábado', '09:00:00', '14:00:00');


INSERT INTO reservas (id_cliente, id_servicio, id_empleado, fecha, hora_inicio, estado, observaciones) VALUES
(8, 1, 1, '2026-03-02', '10:00:00', 'confirmada', 'Cliente habitual'),
(9, 3, 2, '2026-03-03', '11:00:00', 'pendiente', ''),
(10, 2, 1, '2026-03-04', '17:00:00', 'confirmada', 'Cambio de look'),
(11, 4, 3, '2026-03-05', '09:30:00', 'pendiente', 'Color completo'),
(12, 6, 5, '2026-03-06', '18:00:00', 'confirmada', 'Peinado para evento');

INSERT INTO ventas (id_cliente, total) VALUES
(8, 22.45),
(9, 9.95);

INSERT INTO ventas_detalles (id_venta, id_producto, cantidad, precio_unitario) VALUES
(1, 1, 1, 12.50),
(1, 3, 1, 9.95),
(2, 3, 1, 9.95);
