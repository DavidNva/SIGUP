Use UDP_CONTROL;
--El Orden de ejecucion de scripts es: 
/*
  1. Biblioteca-DDL Y triGGERS 
  2. Vistas
  3. Procedimientos Almacenados
  4. Inserciones  
*/
-- --Para cada procedimiento se debe
-- --Categoria: Procedimiento para generar codigo al insertar
-- --      Procedimiento para actualizar
-- --      Procedimiento para eliminar - (para el que lo requiera)
-- go
go

--Procedimientos para categorias
create procedure sp_RegistrarCategoria(--Hay un indice unico para el nombre completo del Categoria 
    --@IDCategoria int,---El id es Identity
    @Descripcion varchar(100),--Tiene indice compuesto con Apellidos
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Categoria_Herramienta WHERE Descripcion = @Descripcion)
    begin 
        insert into Categoria_Herramienta(Descripcion, Activo) values 
        (@Descripcion, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end 
    else 
     SET @Mensaje = 'La categoria ya existe'
end
GO
create  proc sp_EditarCategoria(
    @IdCategoria int,
    @Descripcion varchar(100),--Tiene indice compuesto con Apellidos
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Categoria_Herramienta WHERE Descripcion = @Descripcion and IDCategoria != @IdCategoria)
    begin 
         update top(1) Categoria_Herramienta set 
        Descripcion = @Descripcion,
        Activo = @Activo
        where IDCategoria = @IdCategoria

        set @Resultado = 1 --true
    end 
    else 
       set @Mensaje = 'La categoria ya existe'
end

go
create proc sp_EliminarCategoria( --Trabajo como un booleano
    @IdCategoria int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM Herramienta p --validacion de que la categoria no este relacionada con un producto
    inner join Categoria_Herramienta c on c.IDCategoria = p.Id_Categoria WHERE p.Id_Categoria= @IdCategoria)
    begin 
        delete top(1) from Categoria_Herramienta where IDCategoria = @IdCategoria
        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La categoria se encuentra relacionada con una herramienta'
end
GO

--Procedimientos para las marcas
create procedure sp_RegistrarMarca(--Hay un indice unico para el nombre completo del Categoria 
    --@IDCategoria int,---El id es Identity
    @DescripcionMarca varchar(100),--Tiene indice compuesto con Apellidos
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM marca_herramienta WHERE Descripcion = @DescripcionMarca)
    begin 
        insert into marca_herramienta(Descripcion, Activo) values 
        (@DescripcionMarca, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = scope_identity()
    end 
    else 
     SET @Mensaje = 'La marca ya existe'
end
go

create procedure sp_EditarMarca(
    @IdMarca int,
    @Descripcion varchar(100),--Tiene indice compuesto con Apellidos
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM marca_herramienta WHERE Descripcion = @Descripcion and IdMarca != @IdMarca)
    begin 
         update top(1) marca_herramienta set 
        Descripcion = @Descripcion,
        Activo = @Activo
        where IdMarca = @IdMarca

        set @Resultado = 1 --true
    end 
    else 
       set @Mensaje = 'La marca ya existe'
end
GO

create procedure sp_EliminarMarca(
	@IdMarca int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    IF NOT EXISTS (SELECT * FROM herramienta h --validacion de que la categoria no este relacionada con un producto
    inner join marca_herramienta m on m.IdMarca = h.id_marca WHERE h.id_marca = @IdMarca)
    begin 
        delete top(1) from marca_herramienta where IdMarca = @IdMarca
        set @Resultado = 1 --true
    end 
    else 
        set @Mensaje = 'La categoria se encuentra relacionada con un libro'
end
GO
--inserciones prueba categorias
exec sp_RegistrarMarca 'TRUPER',1,'',1
exec sp_RegistrarMarca 'PRETUL',1,'',1
exec sp_RegistrarMarca 'HYUNDAI',1,'',1
exec sp_RegistrarMarca 'HUSKY',1,'',1
exec sp_RegistrarMarca 'STIHL',1,'',1

go
-- Procedimientos para usuario
CREATE PROC sp_RegistrarUsuario
(
@IdUsuario int,
@Nombre varchar(50),
@Apellidos varchar(50),
@TipoUsuario int
)
AS
INSERT INTO usuario VALUES (@IdUsuario, @Nombre, @Apellidos, @TipoUsuario);
GO

CREATE PROC sp_EditarUsuario
(
@IdUsuario int,
@Nombre varchar(50),
@Apellidos varchar(50),
@TipoUsuario int
)
AS
UPDATE usuario SET Nombre = @Nombre, Apellidos = @Apellidos, Tipo = @TipoUsuario WHERE IdUsuario = @IdUsuario
GO

CREATE PROC sp_EliminarUsuario
(
@IdUsuario int
)
AS
DELETE usuario WHERE IdUsuario = @IdUsuario;
GO

--Inserts de los Tipos de usuarios
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Alumno');
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Docente');
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Visitante externo');

use UDP_Control
go
--inserciones prueba categorias
exec sp_RegistrarCategoria 'HERRAMIENTAS DE MEDICIÓN',1,'',1
exec sp_RegistrarCategoria 'HERRAMIENTAS DE ELECTRICIDAD',1,'',1
exec sp_RegistrarCategoria 'HERRAMIENTAS DE CORTE',1,'',1
exec sp_RegistrarCategoria 'HERRAMIENTAS DE FIJACIÓN',1,'',1

select * from Categoria_Herramienta


go
use UDP_CONTROL

GO
--Procedimientos para Herramienta
create procedure sp_RegistrarHerramienta(--Hay un indice unico para el nombre completo del usuario 
    --@IDUsuario int,---El id es Identity
	 @IdHerramienta varchar(50),--Es asignado por administrador al insertar
    @Nombre nvarchar(60),
    @Cantidad int,
    --Llaves foraneas
    @IDMarca int,
    @IDCategoria int,--Tiene un DEFAULT en Sala = S0001 (Sala General)
    @Observaciones varchar(500), --Definido como default: EN BUEN ESTADO
    @Activo bit,--Mejor al registrarlo darlo por default como 1, no tiene sentido regitrar y darlo como inactivo
    @Mensaje varchar(500) output,
    @Resultado int output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Herramienta WHERE IdHerramienta = @IdHerramienta)
    begin 
        insert into Herramienta(IdHerramienta, Nombre, Cantidad,  id_marca, id_categoria,Observaciones, Activo) values 
            (@IdHerramienta, @Nombre, @Cantidad, @IDMarca, @IDCategoria,  @Observaciones, 1)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la
        SET @Resultado = scope_identity()
    end 
    else 
     SET @Mensaje = 'El código de la herramienta ya existe'
end

go
sp_RegistrarHerramienta 'Mart73r','Martillo',5,1,1,'NINGUNA',1,'',1
GO
create  procedure sp_EditarHerramienta(
    @IdHerramienta varchar(50),--Es asignado por administrador al insertar
    @Nombre nvarchar(60),
    @Cantidad int,
    --Llaves foraneas
    @IDMarca int,
    @IDCategoria int,--Tiene un DEFAULT en Sala = S0001 (Sala General)
    @Observaciones varchar(500), --Definido como default: EN BUEN ESTADO
    @Activo bit,--Mejor al registrarlo darlo por default como 1, no tiene sentido regitrar y darlo como inactivo
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Herramienta WHERE nombre = @Nombre and IdHerramienta != @IdHerramienta)
    begin 
        update Herramienta set
        Nombre = @Nombre,
        Cantidad = @Cantidad,
        id_marca = @IDMarca, 
        id_categoria = @IDCategoria,  
        Observaciones = @Observaciones, 
        Activo = @Activo 
        where IdHerramienta = @IdHerramienta
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
     SET @Mensaje = 'El código de la Herramienta ya existe'
end

go
--PARA QUE ESTO FUNCIONE BIEN, ENTONCES EL DETALLEPRESTAMO EN SU COLUMNA ACTIVO
    --CUANDO EL ADMIN ACTUALICE UN PRESTAMO DE ACTIVO A INACTIVO (ES DECIR QUE VA A DEVOLVER EL Herramienta Y YA NO VA ESTAR EN PRESTAMO)
    --EL ACTIVO DE PRESTAMO SE ACTUALIZA A 0 Y ENTONCES AUTOMAICAMENTE TAMBIEN EL ACTIVO DE DETALLE PRESTAMO DEBE SER 0
    --CON ESO VALIDAREMOS ESTA SELECCION PARA ELIMINAR UN Herramienta NO DEBE HABER UN Herramienta RELACIONADO CON EJEMPLAR CUYO EJEMPLAR ESTE RELACIONADO 
    --A UN DETALLEPRESTAMO CUYO A SU VEZ ESTA RELACIONADO CON PRESTAMO Y ESTE ACTIVO DICHO PRESTAMO. ENTONCES PARA PODER ELMINAR
    --NO DEBE ESTAR UN ID CON UN EJEMPLAR EN DETALLE PRESTAMO QUE AUN ESTE ACTIVO.
go
create  procedure sp_EliminarHerramienta(
    @IdHerramienta varchar(50),
    @Mensaje varchar(500) output,
    @Resultado int output
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (
	select * from herramienta h
    inner join Detalle_Prestamo dp on dp.IDHerramienta = h.IdHerramienta
    inner join Prestamo p on p.IdPrestamo = dp.IdPrestamo and p.Activo = 1
    where h.IdHerramienta = @IdHerramienta)--No podemos eliminar un Herramienta si ya esta incluido en una venta
    begin 
        delete top(1) from Herramienta where IdHerramienta = @IdHerramienta

        -- delete top(1) from Prestamo where IdPrestamo = @IdPrestamo
        --Como el ejemplar tiene una relacion con idHerramienta y un deletecascade se eliminará automaticamente al eliminar el Herramienta
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
     SET @Mensaje = 'La Herramienta se encuentra relacionada a un préstamo'
end 
go
select * from herramienta


--------------------------------- PRESTAMOS -------------------------------------------------------
CREATE TYPE [dbo].[EDetalle_Prestamo] AS TABLE(
	[IdHerramienta] varchar(50) null,
	[Cantidad] int null
)
GO
create procedure usp_RegistrarPrestamo(
    @Id_Usuario int,
    --@IdHerramienta int, /*Por ejemplar*/
    @CantidadTotal int, 
	@Unidad varchar(50),
	@CantidadPU int,
	@AreaDeUso varchar(50),
	@Id_Area int, 
    --@MontoTotal decimal(18,2),
    @DiasDePrestamo int, 
    --@Estado bit,--Es como si dijeramos activo(El 0 significa no Prestamo activo o "DEVUELTO" y
    -- el 1 significa prestamo activo o "No devuelto")
    @Observaciones nvarchar(500),
    @Id_Herramienta varchar(50),--SE AGREGO ESTA COLUMNA PARA PLICAR LA ELIMINACION EN CASCADA EN CASO DE QUE SE ELIMINE EL LIBRO
	--@CalificacionEntrega varchar(50),
    @DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @Resultado bit output,
    @Mensaje varchar(500) output
)
as 
begin 
    begin try 
        declare @idPrestamo int = 0
		declare @cantidadInicial int =  (select cantidad from herramienta where IdHerramienta = @Id_Herramienta)
        set @Resultado = 1
        set @Mensaje = ''
		IF  @cantidadInicial - @CantidadTotal < 0
		begin
		    set @Resultado = 0
			--set @Mensaje = 'Error: La cantidad ingresada es mayor al stock disponible de esa herramienta, intente con una cantidad menor.'
			set @Mensaje = CONCAT('Error: La cantidad ingresada (', @CantidadTotal, ') es mayor al stock disponible (', @cantidadInicial, ') de esa herramienta, intente con una cantidad menor. O actualice la página para ver de nuevo el stock.')
		end
		else
		begin
			begin transaction registro
			insert into Prestamo(IdUsuario, Cantidad,Unidad, CantidadPU, AreaDeUso, Area, DiasDePrestamo, Notas,IdHerramienta )--COLUMNA NUEVA PARA PLICAR EL DELETE CASCADE EN CASO DE QUE SE ELIMINE UN LIBRO
			values(@Id_Usuario, @CantidadTotal,@Unidad, @CantidadPU,@AreaDeUso, @Id_Area, @DiasDePrestamo, @Observaciones,@Id_Herramienta)

			set @idPrestamo = SCOPE_IDENTITY()--obtiene el ultimo id que se esta registrando
		
			insert into Detalle_Prestamo(IdPrestamo, IdHerramienta, Cantidad)
			select @idPrestamo, IdHerramienta, Cantidad from @DetallePrestamo
			--update Ejemplar set Activo = 0 where IDEjemplarLibro = (select IdEjemplar from @EjemplarActivo)
			--update herramienta set Activo = 0 where IDEjemplarLibro = (select IdEjemplar from @DetallePrestamo)
			--update herramienta set cantidad = cantidad - @CantidadTotal where IdHerramienta = @Id_Herramienta
			update herramienta set cantidad = cantidad - 1 where IdHerramienta = @Id_Herramienta
        --DELETE FROM CARRITO WHERE IdLector = @Id_Lector
			commit transaction registro 
		end
		end try 
    begin catch --en el caso de algun error, reestablece todo
        set @Resultado = 0
        set @Mensaje = ERROR_MESSAGE()
        rollback transaction registro 
    end catch 
end


/*Inserciones prueba para prestamos*/
insert into Edificio (NombreEdificio)
values
	  ('Edificio E'),
	  ('Centro de Computo'),
	  ('Unidad de Practicas');
go
select * from Edificio
go

Insert Into Areas(IdArea,NombreArea, IdEdificio)
values(1,'LESO',1),
	  (2,'LABORATORIO A',3),
	  (3,'LABORATORIO B',3);
GO
SELECT * FROM Areas

use UDP_CONTROL

select * from prestamo
go
create procedure sp_EditarPrestamo
(
    @IdPrestamo int,
    @Id_Usuario int,
    @CantidadTotal int,
	@Unidad varchar(50),
	@CantidadPU int,
	@AreaDeUso varchar(100),
	@Id_Area int,
    --@Activo bit,
    @FechaPrestamo date,
    --@FechaDevolucion date,
    @DiasDePrestamo int,
    @Observaciones varchar(500),--notas
    --@DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    SET @Resultado = 1 --No permite repetir un mismo prestamo, ni al insertar ni al actualizar
    SET @Mensaje = '' -- Asignar un valor vacío a la variable @Mensaje

    IF EXISTS (SELECT * FROM Prestamo WHERE IdPrestamo = @IdPrestamo)
    begin 

        -- Convert the input date string to datetime
        DECLARE @FechaPrestamoDatetime datetime
        SET @FechaPrestamoDatetime = CONVERT(datetime, @FechaPrestamo, 3)

        -- DECLARE @FechaDevolucionDatetime datetime
        -- SET @FechaDevolucionDatetime = CONVERT(datetime, @FechaDevolucion, 3)
		
        update Prestamo set
        IdUsuario = @Id_Usuario,
        Cantidad = @CantidadTotal,
		Unidad = @Unidad,
		CantidadPU = @CantidadPU,
		AreaDeUso = @AreaDeUso,
		Area = @Id_Area,
        --Activo = @Activo, 
        FechaPrestamo = @FechaPrestamoDatetime,
        --FechaDevolucion = @FechaDevolucionDatetime,
        DiasDePrestamo = @DiasDePrestamo, 
        Notas = @Observaciones
        where IdPrestamo = @IdPrestamo
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
        SET @Mensaje = 'Error: El préstamo no pudo ser actualizado' + ERROR_MESSAGE() 
end 

go
--go
create procedure sp_FinalizarPrestamo --EditarPrestamo2
(
    @IdPrestamo int,
    --@FechaPrestamo date,
    @FechaDevolucion date,
    @Observaciones varchar(500),
    --@DetallePrestamo [EDetalle_Prestamo] READONLY,--SE USA LA ESTRUCTURA CREADA ANTERIORMENTE
	--@EjemplarActivo [Ejemplar_Activo] READONLY,
    @IdHerramienta varchar(50),--Este y el siguiente será para actualizar a activo el jemplar y el stock sumar1 al libro correspondiente
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    SET @Resultado = 1 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    SET @Mensaje = '' -- Asignar un valor vacío a la variable @Mensaje

    IF EXISTS (SELECT * FROM Prestamo WHERE IdPrestamo = @IdPrestamo)
    begin 

        -- Convert the input date string to datetime
        --DECLARE @FechaPrestamoDatetime datetime
        --SET @FechaPrestamoDatetime = CONVERT(datetime, @FechaPrestamo, 3)

        DECLARE @FechaDevolucionDatetime datetime
        SET @FechaDevolucionDatetime = CONVERT(datetime, @FechaDevolucion, 3)

        update Prestamo set
        Activo = 0,--Es decir prestamo devuelto 
        --FechaPrestamo = @FechaPrestamoDatetime,
        FechaDevolucion = @FechaDevolucionDatetime,
        Notas = @Observaciones
        where IdPrestamo = @IdPrestamo
        --update Herramienta set Activo = 1 where IdHerramienta = @IdHerramienta
        --update Libro set Ejemplares = Ejemplares + 1 where IdLibro = @IdLibro
		update herramienta 
		set cantidad = cantidad + 1,
		activo = 1
		where IdHerramienta = @IdHerramienta
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
        SET @Resultado = 1 --true
    end 
    else 
        SET @Mensaje = 'Error: No se pudo finalizar el préstamo. Intentelo otra vez.'
end 


select * from herramienta
go
create proc sp_EliminarPrestamo( --Trabajo como un booleano
    @IdPrestamo int,
    @IdHerramienta varchar(50), 
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    begin
        delete top(1) from Prestamo where IdPrestamo = @IdPrestamo
       
        update herramienta 
		set cantidad = cantidad + 1,
		activo = 1
		where IdHerramienta = @IdHerramienta
		
        set @Resultado = 1 --true
        
    end 
    if(@Resultado != 1)
        set @Mensaje = 'Error: No se pudo elimnar el préstamo. Intentelo de nuevo'
end

select * from Administrador
select * from usuario
---------------------------------------------ADMINISTRADOR ------------------------------------------
go
create procedure sp_RegistrarAdministrador(--Hay un indice unico para el nombre completo del usuario 
    --@IDUsuario int,---El id es Identity
	@IdAdministrador varchar(30),
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Telefono varchar(20),
    @Correo varchar(100),--Puede ser null
    @Clave varchar(150),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
    --@ID_TipoPersona int --ESTARÁ COMO DEFAULT = 1, ES DECIR, COMO LECTOR
    --FechaCreacion date --Esta como default DEFAULT GETDATE()
    )
as
begin
    SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
    IF NOT EXISTS (SELECT * FROM Administrador WHERE Correo = @Correo and IdAdministrador != @IdAdministrador)
    begin 
        insert into Administrador(IdAdministrador,Nombres, Apellidos, Telefono, Correo, Clave, Activo)
        values (@IdAdministrador,@Nombres, @Apellidos,@Telefono, @Correo, @Clave, @Activo)
        --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la
        SET @Resultado = scope_identity()
    end 
    else 
     SET @Mensaje = 'El correo o id del usuario ya existe'
end

GO
sp_RegistrarAdministrador 'info2024','djhon','David Jhon ','1345678765','davidjhon@gmail.com','test123',1,'',1
go
create proc sp_EditarAdministrador(
    @IdAdministrador varchar(30),
    @Nombres varchar(100),--Tiene indice compuesto con Apellidos
    @Apellidos varchar(100),--Tiene indice compuesto con Nombre
    @Telefono varchar(20),
    @Correo varchar(100),--Puede ser null
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
)
as
begin 
    SET @Resultado = 0
    IF NOT EXISTS (SELECT * FROM Administrador WHERE Correo = @Correo and IdAdministrador != @IdAdministrador)
    begin 
        update top(1) Administrador set 
        Nombres = @Nombres,
        Apellidos  = @Apellidos,
        Telefono = @Telefono,
        Correo = @Correo,
        Activo = @Activo
        where IdAdministrador = @IdAdministrador
        set @Resultado = 1
    end 
    else 
        set @Mensaje = 'El correo del administrador ya existe'
end

go
create proc sp_EliminarAdministrador( --Trabajo como un booleano
    @IdAdministrador varchar(30),
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin 
    SET @Resultado = 0 --false
    begin
        delete top(1) from Administrador where IDAdministrador = @IdAdministrador
        set @Resultado = 1 --true
    end 
    if(@Resultado != 1)
        set @Mensaje = 'Error: No se pudo eliminar el administrador. Intentelo de nuevo'
end


select * from Administrador
