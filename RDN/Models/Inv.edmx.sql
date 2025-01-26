
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/22/2025 16:51:34
-- Generated from EDMX file: C:\Users\Alber\OneDrive\Escritorio\Transacciones\RDN\Models\Inv.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Inventarios];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_InventarioDetalle_Inventarios]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventarioDetalle] DROP CONSTRAINT [FK_InventarioDetalle_Inventarios];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioDetalle_Productos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventarioDetalle] DROP CONSTRAINT [FK_InventarioDetalle_Productos];
GO
IF OBJECT_ID(N'[dbo].[FK_InventarioDetalle_Saldos]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventarioDetalle] DROP CONSTRAINT [FK_InventarioDetalle_Saldos];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[InventarioDetalle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventarioDetalle];
GO
IF OBJECT_ID(N'[dbo].[Inventarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Inventarios];
GO
IF OBJECT_ID(N'[dbo].[Productos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Productos];
GO
IF OBJECT_ID(N'[dbo].[Saldos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Saldos];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'InventarioDetalle'
CREATE TABLE [dbo].[InventarioDetalle] (
    [Folio] int  NOT NULL,
    [Sucursal] varchar(30)  NOT NULL,
    [Renglon] int IDENTITY(1,1) NOT NULL,
    [ProductoID] varchar(30)  NOT NULL,
    [Precio] decimal(19,4)  NOT NULL,
    [Cantidad] float  NOT NULL
);
GO

-- Creating table 'Inventarios'
CREATE TABLE [dbo].[Inventarios] (
    [Folio] int  NOT NULL,
    [Sucursal] varchar(30)  NOT NULL,
    [Fecha] datetime  NOT NULL,
    [Total] float  NOT NULL,
    [TipoMovimiento] varchar(20)  NOT NULL
);
GO

-- Creating table 'Productos'
CREATE TABLE [dbo].[Productos] (
    [ProductoID] varchar(30)  NOT NULL,
    [Descripcion] varchar(100)  NOT NULL,
    [PrecioCompra] decimal(19,4)  NOT NULL,
    [PrecioVenta] decimal(19,4)  NOT NULL
);
GO

-- Creating table 'Saldos'
CREATE TABLE [dbo].[Saldos] (
    [ProductoID] varchar(30)  NOT NULL,
    [Sucursal] varchar(30)  NOT NULL,
    [Saldo] float  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Folio], [Sucursal], [Renglon] in table 'InventarioDetalle'
ALTER TABLE [dbo].[InventarioDetalle]
ADD CONSTRAINT [PK_InventarioDetalle]
    PRIMARY KEY CLUSTERED ([Folio], [Sucursal], [Renglon] ASC);
GO

-- Creating primary key on [Folio], [Sucursal] in table 'Inventarios'
ALTER TABLE [dbo].[Inventarios]
ADD CONSTRAINT [PK_Inventarios]
    PRIMARY KEY CLUSTERED ([Folio], [Sucursal] ASC);
GO

-- Creating primary key on [ProductoID] in table 'Productos'
ALTER TABLE [dbo].[Productos]
ADD CONSTRAINT [PK_Productos]
    PRIMARY KEY CLUSTERED ([ProductoID] ASC);
GO

-- Creating primary key on [ProductoID], [Sucursal] in table 'Saldos'
ALTER TABLE [dbo].[Saldos]
ADD CONSTRAINT [PK_Saldos]
    PRIMARY KEY CLUSTERED ([ProductoID], [Sucursal] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Folio], [Sucursal] in table 'InventarioDetalle'
ALTER TABLE [dbo].[InventarioDetalle]
ADD CONSTRAINT [FK_InventarioDetalle_Inventarios]
    FOREIGN KEY ([Folio], [Sucursal])
    REFERENCES [dbo].[Inventarios]
        ([Folio], [Sucursal])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ProductoID] in table 'InventarioDetalle'
ALTER TABLE [dbo].[InventarioDetalle]
ADD CONSTRAINT [FK_InventarioDetalle_Productos]
    FOREIGN KEY ([ProductoID])
    REFERENCES [dbo].[Productos]
        ([ProductoID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioDetalle_Productos'
CREATE INDEX [IX_FK_InventarioDetalle_Productos]
ON [dbo].[InventarioDetalle]
    ([ProductoID]);
GO

-- Creating foreign key on [ProductoID], [Sucursal] in table 'InventarioDetalle'
ALTER TABLE [dbo].[InventarioDetalle]
ADD CONSTRAINT [FK_InventarioDetalle_Saldos]
    FOREIGN KEY ([ProductoID], [Sucursal])
    REFERENCES [dbo].[Saldos]
        ([ProductoID], [Sucursal])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventarioDetalle_Saldos'
CREATE INDEX [IX_FK_InventarioDetalle_Saldos]
ON [dbo].[InventarioDetalle]
    ([ProductoID], [Sucursal]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------