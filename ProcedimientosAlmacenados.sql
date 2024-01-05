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
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Administrador');
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Usuario');
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Alumno');
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Docente');
INSERT INTO tipo_usuario(nombre_tipo) VALUES ('Visitante externo');

use UDP_Control
--inserciones prueba
exec sp_RegistrarCategoria 'CONSULTA',1,'',1
exec sp_RegistrarCategoria 'GENERALIDADES',1,'',1
exec sp_RegistrarCategoria 'LITERATURA',1,'',1
exec sp_RegistrarCategoria 'FILOSOFÍA Y PSICOLOGÍA',1,'',1
exec sp_RegistrarCategoria 'RELIGIONES',1,'',1
exec sp_RegistrarCategoria 'CIENCIAS SOCIALES',1,'',1
exec sp_RegistrarCategoria 'CIENCIAS PURAS',1,'',1
exec sp_RegistrarCategoria 'CIENCIAS APLICADAS',1,'',1
exec sp_RegistrarCategoria 'BELLAS ARTES',1,'',1
exec sp_RegistrarCategoria 'GEOGRAFÍA E HISTORIA',1,'',1
exec sp_RegistrarCategoria 'NOVELAS',1,'',1
exec sp_RegistrarCategoria 'POESÍA',1,'',1
exec sp_RegistrarCategoria 'CUENTOS',1,'',1
exec sp_RegistrarCategoria 'BIOGRAFÍAS',1,'',1
exec sp_RegistrarCategoria 'MÉXICO',1,'',1
exec sp_RegistrarCategoria 'PUEBLA',1,'',1
exec sp_RegistrarCategoria 'LIBROS DONADOS',1,'',1
exec sp_RegistrarCategoria 'LIBROS INFANTILES',1,'',1

select * from Categoria_Herramienta


go
use UDP_CONTROL

GO

--Procedimientos para Herramienta
create procedure sp_RegistrarHerramienta(--Nuevo sp que registra igual el Herramienta con su ejemplar  la vez
    @IdHerramienta int,--Es asignado por administrador al insertar
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
    begin try 
        declare @idCodigoHerramienta int = 0
        SET @Resultado = 0 --No permite repetir un mismo correo, ni al insertar ni al actualizar
        IF NOT EXISTS (SELECT * FROM Herramienta WHERE IdHerramienta = @IdHerramienta)
        begin
            begin transaction registroHerramienta
            insert into Herramienta(IdHerramienta, Nombre, Cantidad,  id_marca, id_categoria,Observaciones, Activo) values 
            (@IdHerramienta, @Nombre, @Cantidad, @IDMarca, @IDCategoria,  @Observaciones, 1)
            --La función SCOPE_IDENTITY() devuelve el último ID generado para cualquier tabla de la sesión activa y en el ámbito actual.
            SET @Resultado = scope_identity() 
            set @idCodigoHerramienta = SCOPE_IDENTITY()--obtiene el ultimo id que se esta registrando
            
            --insert into Ejemplar(ID_Herramienta, Activo)
            --values(@@idCodigoHerramienta,1)
            commit transaction registroHerramienta
        end
        else 
        SET @Mensaje = 'El código de la Herramienta ya existe*'
    end try
    begin catch
        set @Resultado = 0
        set @Mensaje = ERROR_MESSAGE()
        rollback transaction registroHerramienta 
    end catch
    
end 
go
create procedure sp_EditarHerramienta(
    @IdHerramienta int,--Es asignado por administrador al insertar
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
create procedure sp_EliminarHerramienta(
    @IdHerramienta int,
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
