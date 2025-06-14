CREATE DATABASE [WorkDb]
GO
USE [WorkDb]
GO
/****** Object:  Table [dbo].[FtpConfigurations]    Script Date: 29.02.2024 18:03:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FtpConfigurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Login] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Port] [int] NOT NULL,
 CONSTRAINT [PK_FtpConfigurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FtpFiles]    Script Date: 29.02.2024 18:03:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FtpFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceActionId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FtpFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FtpServicesActions]    Script Date: 29.02.2024 18:03:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FtpServicesActions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NOT NULL,
	[ActionName] [nvarchar](450) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_FtpServicesActions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServicesPermisions]    Script Date: 29.02.2024 18:03:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServicesPermisions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_ServicesPermisions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FtpConfigurations]  WITH CHECK ADD  CONSTRAINT [FK_FtpConfigurations_ServicesPermisions_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[ServicesPermisions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FtpConfigurations] CHECK CONSTRAINT [FK_FtpConfigurations_ServicesPermisions_ServiceId]
GO
ALTER TABLE [dbo].[FtpFiles]  WITH CHECK ADD  CONSTRAINT [FK_FtpFiles_FtpServicesActions_ServiceActionId] FOREIGN KEY([ServiceActionId])
REFERENCES [dbo].[FtpServicesActions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FtpFiles] CHECK CONSTRAINT [FK_FtpFiles_FtpServicesActions_ServiceActionId]
GO
ALTER TABLE [dbo].[FtpServicesActions]  WITH CHECK ADD  CONSTRAINT [FK_FtpServicesActions_ServicesPermisions_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[ServicesPermisions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FtpServicesActions] CHECK CONSTRAINT [FK_FtpServicesActions_ServicesPermisions_ServiceId]
GO
