--drop database  testLibrary
if not exists(select * from sys.databases where name='testLibrary')
begin
	create database testLibrary
end
go
use  testLibrary
go
if not exists(select * from sys.objects where name='Usuario' and type='U')
begin
	create table Usuario
	(Id int IDENTITY(1,1) PRIMARY KEY,
	Usuario varchar(20) not null,
	Contrasena varchar(8) not null,
	Intentos int null,
	NivelSeg decimal null,
	FechaReg datetime null
	)
end
go
if not exists(select * from sys.objects where name='autores' and type='U')
begin
	create table autores
	(id int IDENTITY(1,1) PRIMARY KEY,
	nombre varchar(45) not null,
	apellidos varchar(45) not null
	)
end
go
if not exists(select * from sys.objects where name='editoriales' and type='U')
begin
	create table editoriales
	(id int IDENTITY(1,1) PRIMARY KEY,
	nombre varchar(45) not null,
	sede varchar(45) not null
	)
end
go
if not exists(select * from sys.objects where name='libros' and type='U')
begin
	create table libros
	(ISBN int IDENTITY(1,1) PRIMARY KEY,
	editoriales_id int not null,
	titulo varchar(45) not null,
	sinopsis varchar(100) not null,
	n_paginas varchar(45) not null,
	CONSTRAINT fk_editoriales FOREIGN KEY (editoriales_id)
        REFERENCES editoriales (id),
	)
end
go
if not exists(select * from sys.objects where name='autores_has_libros' and type='U')
begin
	create table autores_has_libros
	(autores_id int not null,
	libros_ISBN int not null,
	CONSTRAINT fk_autores FOREIGN KEY (autores_id)
        REFERENCES autores (id),
	CONSTRAINT fk_libros FOREIGN KEY (libros_ISBN)
        REFERENCES libros (ISBN)
	)
end
go


insert into autores(nombre,apellidos) values('GABRIEL','GARCIA MARQUEZ')
GO
insert into autores(nombre,apellidos) values('PEPITO','GARCIA PEREZ')
GO

INSERT INTO editoriales(nombre, sede) values('prueba','mexico')
go
insert into libros(editoriales_id,titulo,sinopsis,n_paginas) values(1,'100 ANIOS DE SOLEDAD','PRUEBA','200')
GO

insert into libros(editoriales_id,titulo,sinopsis,n_paginas) values(1,'PRUEBA','PRUEBA','200')
GO
insert into autores_has_libros(autores_id,libros_ISBN) values(1,1)
go

USE testLibrary
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
if exists(select * from sys.objects where type='P' and name='PROCEDURE_TRAVEL')
BEGIN
	DROP PROCEDURE PROCEDURE_TRAVEL
end
go

CREATE PROCEDURE [dbo].[PROCEDURE_TRAVEL]
	@usuario VARCHAR	(50) null,
	@contrasena VARCHAR	(250) null,
	@ISBN int null,
	@nombre varchar(100) null,
	@apellidos varchar(100) null,
	@nombre_editorial VARCHAR(100) null,
	@sede VARCHAR(100) null,
	@titulo VARCHAR(100) null,
	@sinopsis VARCHAR(100) null,
	@n_paginas int null,
	@operacion char(1) 

AS
BEGIN

	declare @editoriales_id int,
			@autores_id int
	

	if @operacion='C'
	begin
		SELECT usuario, contrasena
		FROM   Usuario 
		WHERE usuario = @usuario --AND contrasena = @contrasena
	end
	
	if @operacion='I'
	begin
		insert into Usuario(Usuario,Contrasena,Intentos,NivelSeg,FechaReg)
		select @usuario,@contrasena,0,1,getdate()
		
		SELECT usuario, contrasena
		FROM   Usuario 
		WHERE usuario = @usuario --AND contrasena = @contrasena
	end
	
	if @operacion='P'
	begin
		if exists(select *
					from autores a 
					inner join autores_has_libros al on al.autores_id=a.id
					inner join libros l on l.ISBN=al.libros_ISBN
					inner join editoriales e on e.id= l.editoriales_id where l.ISBN= @ISBN)
		BEGIN

			select @editoriales_id = isnull(editoriales_id,0)
			from libros
			WHERE ISBN= @ISBN

			select @autores_id= isnull(autores_id,0)
			from autores_has_libros
			WHERE libros_ISBN= @ISBN

			
				update editoriales
				SET nombre = UPPER(@nombre_editorial),
					sede=UPPER(@sede)
				where id= @editoriales_id

				update autores
				SET nombre = UPPER(@nombre),
					apellidos=UPPER(@apellidos)
				where id= @autores_id

				update libros
				SET editoriales_id=@editoriales_id,
					titulo=UPPER(@titulo),
					sinopsis=UPPER(@sinopsis),
					n_paginas=@n_paginas
				WHERE ISBN= @ISBN


			
		end
		ELSE
		begin

			select @editoriales_id = isnull(id,0)
			from editoriales
			where rtrim(ltrim(@nombre_editorial))= UPPER(@nombre_editorial)

			select @autores_id= isnull(id,0)
			from autores 
			where nombre= upper(nombre) and apellidos= upper(apellidos)
				
				if(@autores_id=0)
				begin
					insert into autores(nombre,apellidos)
					select UPPER(@nombre),UPPER(@apellidos)

					select @autores_id= isnull(id,0)
					from autores 
					where nombre= upper(nombre) and apellidos= upper(apellidos)
				end

				if(@editoriales_id=0)
				begin
					INSERT INTO editoriales(nombre, sede) 
					select @nombre_editorial,@sede

					select @editoriales_id = isnull(id,0)
					from editoriales
					where rtrim(ltrim(@nombre_editorial))= UPPER(@nombre_editorial)
				end
					
				insert into libros(editoriales_id,titulo,sinopsis,n_paginas) 
				select @editoriales_id,@titulo,@sinopsis,@n_paginas
			
				if not exists(select * from autores_has_libros WHERE autores_id=@autores_id AND libros_ISBN=@ISBN)
				begin
					insert into autores_has_libros(autores_id,libros_ISBN) 
					select @autores_id,@ISBN
				end
			
		end
		
		select l.ISBN,a.nombre, a.apellidos,l.titulo,l.sinopsis,l.n_paginas, e.nombre as nombre_editorial, e.sede 
					from autores a 
					inner join autores_has_libros al on al.autores_id=a.id
					inner join libros l on l.ISBN=al.libros_ISBN
					inner join editoriales e on e.id= l.editoriales_id 
					where l.ISBN= @ISBN
	end
	
END
