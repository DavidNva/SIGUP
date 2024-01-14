USE master;
CREATE  DATABASE UDP_CONTROL;
GO
USE UDP_CONTROL;
go
CREATE TABLE Administrador(
  IdAdministrador varchar(30) not null CONSTRAINT PK_Administrador PRIMARY KEY,
  Nombres nvarchar(100) not null,
  Apellidos varchar(100) not null,
  Telefono varchar(20) not null, 
  Correo nvarchar(100) not null,
  Clave nvarchar(100) not null, --Contrase�as encriptadas
  Reestablecer bit default 1 not null, -- Por default 1
  Activo bit default 1 not null,
  FechaRegistro datetime default getdate()
)

CREATE TABLE tipo_usuario(
IdTipo int IDENTITY(1,1) PRIMARY KEY not null,
nombre_tipo varchar(20)
);

CREATE TABLE usuario(
IdUsuario varchar(50) PRIMARY KEY  not null,
Nombre varchar(50) not null,
Apellidos varchar(100) not null,
Tipo int constraint FK_Tipo foreign key (Tipo) references tipo_usuario(IdTipo) not null,
);

create table Edificio
(
IdEdificio int Identity(1,1) primary key  not null,
NombreEdificio varchar(60) not null
);
Go

Create Table Carreras
(
IdCarrera int identity(1,1) primary key not null,
NombreCarrera varchar(100) not null
);
Go

create table Areas
(
IdArea int primary key not null, 
NombreArea varchar(60) not null, 
IdEdificio int constraint FK_Edificio foreign key (IdEdificio) references Edificio(IdEdificio) not null 
);
Go


CREATE TABLE marca_herramienta(
IdMarca  int IDENTITY(1,1) not null CONSTRAINT PK_Marca PRIMARY KEY,
Descripcion nvarchar(100) not null,
Activo bit default 1 not null,
FechaRegistro datetime default getdate()
);

CREATE TABLE categoria_herramienta(
IdCategoria  int IDENTITY(1,1) not null CONSTRAINT PK_Categoria PRIMARY KEY,--Esta referenciado con libro (No se permite eliminar)
Descripcion nvarchar(100) not null,
Activo bit default 1 not null,
FechaRegistro datetime default getdate()
);

CREATE TABLE herramienta(
IdHerramienta varchar(50) PRIMARY KEY not null,
nombre varchar(25) not null,
cantidad int not null,
activo bit default 1 not null,
observaciones varchar(200) DEFAULT 'Ninguna' not null,
FechaRegistro datetime default getdate(),
id_marca int CONSTRAINT FK_Marca FOREIGN KEY(id_marca)  REFERENCES marca_herramienta(IdMarca),
id_categoria int CONSTRAINT FK_Categoria FOREIGN KEY(id_categoria)  REFERENCES categoria_herramienta(IdCategoria)
);


CREATE TABLE prestamo(
IdPrestamo int identity (1,1)  not null CONSTRAINT PK_Prestamo PRIMARY KEY,
IdUsuario varchar(50) not null CONSTRAINT FK_Usuario FOREIGN KEY(IdUsuario) REFERENCES usuario(IdUsuario) ON DELETE CASCADE,
Cantidad decimal not null,
Unidad varchar(20) not null, -- Combobox con opciones de pieza kit juego furbol
CantidadPU int null,
AreaDeUso varchar(50), --Combobox instituto o exterior
Area int constraint FK_Area_Prestamo foreign key (Area) references Areas(IdArea),
Activo bit default 1 not null,
FechaPrestamo datetime default getdate(),
FechaDevolucion date null,
DiasDePrestamo int not null default 1,
Notas varchar(500) not null,
IdHerramienta varchar(50) not null CONSTRAINT FK_HerramientaP foreign key (IdHerramienta) references herramienta(IdHerramienta) ON DELETE CASCADE,
CalificacionEntrega int DEFAULT 0 -- Nivel de evaluacion para mostrar historial 0 standy 1 Malo 2 Regular 3 Bueno
);


CREATE TABLE detalle_prestamo(
IdDetallePrestamo int identity(1,1)  not null  CONSTRAINT PK_DetallePrestamo primary key,
IdPrestamo int not null constraint FK_PrestamoDP foreign key (IdPrestamo) references prestamo(IdPrestamo) ON DELETE CASCADE ON UPDATE CASCADE,
IdHerramienta varchar(50) constraint FK_HerramientaDP foreign key (IdHerramienta) references herramienta(IdHerramienta) not null,
Cantidad decimal not null
);


Create Table ControlUsuario(
IdRegistro int identity(1,1) primary key not null,
fecha date default CONVERT(DATE, GETDATE()),
IdUsuario varchar(50) constraint FK_usuarioCU foreign key (IdUsuario) references usuario(IdUsuario) not null,
HoraEntrada time default FORMAT(GETDATE(), 'HH:mm:ss') , 
HoraSalida time ,
TipoActividad varchar(100) not null, 
CantidadAlumnos int not null,
Semestre int not null,
IdCarrera int constraint FK_Carreras foreign key (IdCarrera) references Carreras(IdCarrera) not null
);
Go

Create Table Bitacora
(
IdPractica int identity(1,1) primary key not null,
NombreActividad varchar(100) not null,  
IdRegistro int constraint FK_ControlUsuario foreign key (IdRegistro) references ControlUsuario(IdRegistro) not null,
Observaciones varchar(150) not null
);
Go
