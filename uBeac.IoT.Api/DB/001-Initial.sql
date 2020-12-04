USE [uBeac]
GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Province] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[Latitude] [decimal](12, 9) NULL,
	[Longitude] [decimal](12, 9) NULL,
 CONSTRAINT [PK_Buildings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BuildingZones]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuildingZones](
	[Id] [uniqueidentifier] NOT NULL,
	[BuildingId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[PlanFileId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BuildingZones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[Uid] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[BuildingZoneId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Extension] [nvarchar](max) NOT NULL,
	[Size] [bigint] NOT NULL,
 CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GatewayData]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GatewayData](
	[Id] [uniqueidentifier] NOT NULL,
	[TraceId] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[GatewayId] [uniqueidentifier] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Bytes] [varbinary](max) NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[Protocol] [nvarchar](max) NOT NULL,
	[Method] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GatewayData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GatewayHttpHeaders]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GatewayHttpHeaders](
	[Id] [uniqueidentifier] NOT NULL,
	[GatewayId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GatewayHttpHeaders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GatewayIPRestrictions]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GatewayIPRestrictions](
	[Id] [uniqueidentifier] NOT NULL,
	[GatewayId] [uniqueidentifier] NOT NULL,
	[IP] [nvarchar](max) NOT NULL,
	[Allowed] [bit] NOT NULL,
 CONSTRAINT [PK_GatewayIPRestrictions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gateways]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gateways](
	[Id] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[Uid] [nvarchar](max) NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[BuildingZoneId] [uniqueidentifier] NULL,
	[HttpEnabled] [bit] NOT NULL,
	[HttpSecureOnly] [bit] NOT NULL,
	[MqttEnabled] [bit] NOT NULL,
	[MqttUsername] [nvarchar](max) NOT NULL,
	[MqttPassword] [nvarchar](max) NOT NULL,
	[MqttSecureOnly] [bit] NOT NULL,
 CONSTRAINT [PK_Gateways] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturers]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturers](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Manufacturers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [uniqueidentifier] NOT NULL,
	[ManufacturerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Processor] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sensors]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sensors](
	[Id] [uniqueidentifier] NOT NULL,
	[Uid] [nvarchar](max) NOT NULL,
	[DeviceId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Type] [int] NOT NULL,
	[Prefix] [int] NOT NULL,
	[Unit] [int] NOT NULL,
	[SchemaJSON] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Sensors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SensorSchemas]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensorSchemas](
	[Id] [uniqueidentifier] NOT NULL,
	[SensorId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SensorSchemas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SensorSchemaValues]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensorSchemaValues](
	[Id] [uniqueidentifier] NOT NULL,
	[SensorSchemaId] [uniqueidentifier] NOT NULL,
	[GatewayId] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Value] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_SensorSchemaValues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teams]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[Id] [uniqueidentifier] NOT NULL,
	[Uid] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeamTokens]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TeamTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeamUsers]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeamUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[TeamId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TeamUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 2020-12-04 2:37:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD  CONSTRAINT [FK_Buildings_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Buildings] CHECK CONSTRAINT [FK_Buildings_Teams]
GO
ALTER TABLE [dbo].[BuildingZones]  WITH CHECK ADD  CONSTRAINT [FK_BuildingZones_Buildings] FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([Id])
GO
ALTER TABLE [dbo].[BuildingZones] CHECK CONSTRAINT [FK_BuildingZones_Buildings]
GO
ALTER TABLE [dbo].[BuildingZones]  WITH CHECK ADD  CONSTRAINT [FK_BuildingZones_Files] FOREIGN KEY([PlanFileId])
REFERENCES [dbo].[Files] ([Id])
GO
ALTER TABLE [dbo].[BuildingZones] CHECK CONSTRAINT [FK_BuildingZones_Files]
GO
ALTER TABLE [dbo].[Devices]  WITH CHECK ADD  CONSTRAINT [FK_Devices_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Devices] CHECK CONSTRAINT [FK_Devices_Teams]
GO
ALTER TABLE [dbo].[GatewayData]  WITH CHECK ADD  CONSTRAINT [FK_GatewayData_Gateways] FOREIGN KEY([GatewayId])
REFERENCES [dbo].[Gateways] ([Id])
GO
ALTER TABLE [dbo].[GatewayData] CHECK CONSTRAINT [FK_GatewayData_Gateways]
GO
ALTER TABLE [dbo].[GatewayHttpHeaders]  WITH CHECK ADD  CONSTRAINT [FK_GatewayHttpHeaders_Gateways] FOREIGN KEY([GatewayId])
REFERENCES [dbo].[Gateways] ([Id])
GO
ALTER TABLE [dbo].[GatewayHttpHeaders] CHECK CONSTRAINT [FK_GatewayHttpHeaders_Gateways]
GO
ALTER TABLE [dbo].[GatewayIPRestrictions]  WITH CHECK ADD  CONSTRAINT [FK_GatewayIPRestrictions_Gateways] FOREIGN KEY([GatewayId])
REFERENCES [dbo].[Gateways] ([Id])
GO
ALTER TABLE [dbo].[GatewayIPRestrictions] CHECK CONSTRAINT [FK_GatewayIPRestrictions_Gateways]
GO
ALTER TABLE [dbo].[Gateways]  WITH CHECK ADD  CONSTRAINT [FK_Gateways_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO
ALTER TABLE [dbo].[Gateways] CHECK CONSTRAINT [FK_Gateways_Products]
GO
ALTER TABLE [dbo].[Gateways]  WITH CHECK ADD  CONSTRAINT [FK_Gateways_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[Gateways] CHECK CONSTRAINT [FK_Gateways_Teams]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Manufacturers] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturers] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Manufacturers]
GO
ALTER TABLE [dbo].[Sensors]  WITH CHECK ADD  CONSTRAINT [FK_Sensors_Devices] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Devices] ([Id])
GO
ALTER TABLE [dbo].[Sensors] CHECK CONSTRAINT [FK_Sensors_Devices]
GO
ALTER TABLE [dbo].[SensorSchemas]  WITH CHECK ADD  CONSTRAINT [FK_SensorSchemas_Sensors] FOREIGN KEY([SensorId])
REFERENCES [dbo].[Sensors] ([Id])
GO
ALTER TABLE [dbo].[SensorSchemas] CHECK CONSTRAINT [FK_SensorSchemas_Sensors]
GO
ALTER TABLE [dbo].[SensorSchemaValues]  WITH CHECK ADD  CONSTRAINT [FK_SensorSchemaValues_Gateways] FOREIGN KEY([GatewayId])
REFERENCES [dbo].[Gateways] ([Id])
GO
ALTER TABLE [dbo].[SensorSchemaValues] CHECK CONSTRAINT [FK_SensorSchemaValues_Gateways]
GO
ALTER TABLE [dbo].[SensorSchemaValues]  WITH CHECK ADD  CONSTRAINT [FK_SensorSchemaValues_SensorSchemas] FOREIGN KEY([SensorSchemaId])
REFERENCES [dbo].[SensorSchemas] ([Id])
GO
ALTER TABLE [dbo].[SensorSchemaValues] CHECK CONSTRAINT [FK_SensorSchemaValues_SensorSchemas]
GO
ALTER TABLE [dbo].[TeamTokens]  WITH CHECK ADD  CONSTRAINT [FK_TeamTokens_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamTokens] CHECK CONSTRAINT [FK_TeamTokens_Teams]
GO
ALTER TABLE [dbo].[TeamUsers]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_Teams] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Teams] ([Id])
GO
ALTER TABLE [dbo].[TeamUsers] CHECK CONSTRAINT [FK_TeamUsers_Teams]
GO
ALTER TABLE [dbo].[TeamUsers]  WITH CHECK ADD  CONSTRAINT [FK_TeamUsers_UserProfiles] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfiles] ([Id])
GO
ALTER TABLE [dbo].[TeamUsers] CHECK CONSTRAINT [FK_TeamUsers_UserProfiles]
GO
