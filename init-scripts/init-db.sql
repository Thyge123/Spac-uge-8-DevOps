USE [master]
GO
/****** Object:  Database [Cereals]    Script Date: 14-04-2025 10:30:36 ******/
CREATE DATABASE [Cereals]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Cereals', FILENAME = N'/var/opt/mssql/data/Cereals.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Cereals_log', FILENAME = N'/var/opt/mssql/data/Cereals_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Cereals] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Cereals].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Cereals] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Cereals] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Cereals] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Cereals] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Cereals] SET ARITHABORT OFF 
GO
ALTER DATABASE [Cereals] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Cereals] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Cereals] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Cereals] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Cereals] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Cereals] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Cereals] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Cereals] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Cereals] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Cereals] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Cereals] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Cereals] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Cereals] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Cereals] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Cereals] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Cereals] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Cereals] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Cereals] SET RECOVERY FULL 
GO
ALTER DATABASE [Cereals] SET  MULTI_USER 
GO
ALTER DATABASE [Cereals] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Cereals] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Cereals] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Cereals] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Cereals] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Cereals] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Cereals', N'ON'
GO
ALTER DATABASE [Cereals] SET QUERY_STORE = ON
GO
ALTER DATABASE [Cereals] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Cereals]
GO
/****** Object:  User [admin]    Script Date: 14-04-2025 10:30:36 ******/
CREATE USER [admin] FOR LOGIN [admin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [admin]
GO
ALTER ROLE [db_accessadmin] ADD MEMBER [admin]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [admin]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [admin]
GO
ALTER ROLE [db_datareader] ADD MEMBER [admin]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [admin]
GO
/****** Object:  Table [dbo].[Cereals]    Script Date: 14-04-2025 10:30:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cereals](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[mfr] [char](1) NULL,
	[type] [char](1) NULL,
	[calories] [int] NULL,
	[protein] [int] NULL,
	[fat] [int] NULL,
	[sodium] [int] NULL,
	[fiber] [real] NULL,
	[carbo] [real] NULL,
	[sugars] [int] NULL,
	[potass] [int] NULL,
	[vitamins] [int] NULL,
	[shelf] [int] NULL,
	[weight] [real] NULL,
	[cups] [real] NULL,
	[rating] [real] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14-04-2025 10:30:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[Password] [nvarchar](256) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cereals] ON 

INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (1, N'100% Bran', N'G', N'C', 120, 3, 1, 210, 2, 21, 5, 100, 25, 3, 1, 0.75, 44.5)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (2, N'100% Natural Bran', N'Q', N'C', 120, 3, 5, 15, 2, 8, 8, 135, 0, 3, 1, 1, 3.398368E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (3, N'All-Bran', N'K', N'C', 70, 4, 1, 260, 9, 7, 5, 320, 25, 3, 1, 0.33, 59425504)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (4, N'All-Bran with Extra Fiber', N'K', N'C', 50, 4, 0, 140, 14, 8, 0, 330, 25, 3, 1, 0.5, 9.370491E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (5, N'Almond Delight', N'R', N'C', 110, 2, 2, 200, 1, 14, 8, -1, 25, 3, 1, 0.75, 34384844)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (6, N'Apple Cinnamon Cheerios', N'G', N'C', 110, 2, 2, 180, 1.5, 10.5, 10, 70, 25, 1, 1, 0.75, 2.950954E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (7, N'Apple Jacks', N'K', N'C', 110, 2, 0, 125, 1, 11, 14, 30, 25, 2, 1, 1, 33174094)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (8, N'Basic 4', N'G', N'C', 130, 3, 2, 210, 2, 18, 8, 100, 25, 3, 1.33, 0.75, 3.703856E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (9, N'Bran Chex', N'R', N'C', 90, 2, 1, 200, 4, 15, 6, 125, 25, 1, 1, 0.67, 49120252)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (10, N'Bran Flakes', N'P', N'C', 90, 3, 0, 210, 5, 13, 5, 190, 25, 3, 1, 0.67, 53313812)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (11, N'Cap''n''Crunch', N'Q', N'C', 120, 1, 2, 220, 0, 12, 12, 35, 25, 2, 1, 0.75, 18042852)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (12, N'Cheerios', N'G', N'C', 110, 6, 2, 290, 2, 17, 1, 105, 25, 1, 1, 1.25, 5.0765E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (13, N'Cinnamon Toast Crunch', N'G', N'C', 120, 1, 3, 210, 0, 13, 9, 45, 25, 2, 1, 0.75, 19823572)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (14, N'Clusters', N'G', N'C', 110, 3, 2, 140, 2, 13, 7, 105, 25, 3, 1, 0.5, 4.040021E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (15, N'Cocoa Puffs', N'G', N'C', 110, 1, 1, 180, 0, 12, 13, 55, 25, 2, 1, 1, 22736446)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (16, N'Corn Chex', N'R', N'C', 110, 2, 0, 280, 0, 22, 3, 25, 25, 1, 1, 1, 4.144502E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (17, N'Corn Flakes', N'K', N'C', 100, 2, 0, 290, 1, 21, 2, 35, 25, 1, 1, 1, 45863324)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (18, N'Corn Pops', N'K', N'C', 110, 1, 0, 90, 1, 13, 12, 20, 25, 2, 1, 1, 3.578279E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (19, N'Count Chocula', N'G', N'C', 110, 1, 1, 180, 0, 12, 13, 65, 25, 2, 1, 1, 22396512)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (20, N'Cracklin'' Oat Bran', N'K', N'C', 110, 3, 3, 140, 4, 10, 7, 160, 25, 3, 1, 0.5, 40448772)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (21, N'Cream of Wheat (Quick)', N'N', N'H', 100, 3, 0, 80, 1, 21, 0, -1, 0, 2, 1, 1, 64533816)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (22, N'Crispix', N'K', N'C', 110, 2, 0, 220, 1, 21, 3, 30, 25, 3, 1, 1, 46895644)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (23, N'Crispy Wheat & Raisins', N'G', N'C', 100, 2, 1, 140, 2, 11, 10, 120, 25, 3, 1, 0.75, 36176196)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (24, N'Double Chex', N'R', N'C', 100, 2, 0, 190, 1, 18, 5, 80, 25, 3, 1, 0.75, 44330856)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (25, N'Froot Loops', N'K', N'C', 110, 2, 1, 125, 1, 11, 13, 30, 25, 2, 1, 1, 32207582)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (26, N'Frosted Flakes', N'K', N'C', 110, 1, 0, 200, 1, 14, 11, 25, 25, 1, 1, 0.75, 31435972)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (27, N'Frosted Mini-Wheats', N'K', N'C', 100, 3, 0, 0, 3, 14, 7, 100, 25, 2, 1, 0.8, 5.834514E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (28, N'Fruit & Fibre Dates, Walnuts, and Oats', N'P', N'C', 120, 3, 2, 160, 5, 12, 10, 200, 25, 3, 1.25, 0.67, 4.091705E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (29, N'Fruitful Bran', N'K', N'C', 120, 3, 0, 240, 5, 14, 12, 190, 25, 3, 1.33, 0.67, 41015492)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (30, N'Fruity Pebbles', N'P', N'C', 110, 1, 1, 135, 0, 13, 12, 25, 25, 2, 1, 0.75, 28025764)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (31, N'Golden Crisp', N'P', N'C', 100, 2, 0, 45, 0, 11, 15, 40, 25, 1, 1, 0.88, 35252444)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (32, N'Golden Grahams', N'G', N'C', 110, 1, 1, 280, 0, 15, 9, 45, 25, 2, 1, 0.75, 23804044)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (33, N'Grape Nuts Flakes', N'P', N'C', 100, 3, 1, 140, 3, 15, 5, 85, 25, 3, 1, 0.88, 52076896)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (34, N'Grape-Nuts', N'P', N'C', 110, 3, 0, 170, 3, 17, 3, 90, 25, 3, 1, 0.25, 5.337101E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (35, N'Great Grains Pecan', N'P', N'C', 120, 3, 3, 75, 3, 13, 4, 100, 25, 3, 1, 0.33, 45811716)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (36, N'Honey Graham Ohs', N'Q', N'C', 120, 1, 2, 220, 1, 12, 11, 45, 25, 2, 1, 1, 21871292)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (37, N'Honey Nut Cheerios', N'G', N'C', 110, 3, 1, 250, 1.5, 11.5, 10, 90, 25, 1, 1, 0.75, 31072216)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (38, N'Honey-comb', N'P', N'C', 110, 1, 0, 180, 0, 14, 11, 35, 25, 1, 1, 1.33, 28742414)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (39, N'Just Right Crunchy  Nuggets', N'K', N'C', 110, 2, 1, 170, 1, 17, 6, 60, 100, 3, 1, 1, 36523684)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (40, N'Just Right Fruit & Nut', N'K', N'C', 140, 3, 1, 170, 2, 20, 9, 95, 100, 3, 1.3, 0.75, 3.647151E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (41, N'Kix', N'G', N'C', 110, 2, 1, 260, 0, 21, 3, 40, 25, 2, 1, 1.5, 3.924111E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (42, N'Life', N'Q', N'C', 100, 4, 2, 150, 2, 12, 6, 95, 25, 2, 1, 0.67, 4.532807E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (43, N'Lucky Charms', N'G', N'C', 110, 2, 1, 180, 0, 12, 12, 55, 25, 2, 1, 1, 26734516)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (44, N'Maypo', N'A', N'H', 100, 4, 1, 0, 0, 16, 3, 95, 25, 2, 1, 1, 54850916)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (45, N'Muesli Raisins, Dates, & Almonds', N'R', N'C', 150, 4, 3, 95, 3, 16, 11, 170, 25, 3, 1, 1, 37136864)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (46, N'Muesli Raisins, Peaches, & Pecans', N'R', N'C', 150, 4, 3, 150, 3, 16, 11, 170, 25, 3, 1, 1, 34139764)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (47, N'Mueslix Crispy Blend', N'K', N'C', 160, 3, 2, 150, 3, 17, 13, 160, 25, 3, 1.5, 0.67, 30313352)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (48, N'Multi-Grain Cheerios', N'G', N'C', 100, 2, 1, 220, 2, 15, 6, 90, 25, 1, 1, 1, 40105964)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (49, N'Nut&Honey Crunch', N'K', N'C', 120, 2, 1, 190, 0, 15, 9, 40, 25, 2, 1, 0.67, 29924284)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (50, N'Nutri-Grain Almond-Raisin', N'K', N'C', 140, 3, 2, 220, 3, 21, 7, 130, 25, 3, 1.33, 0.67, 4.069232E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (51, N'Nutri-grain Wheat', N'K', N'C', 90, 3, 0, 170, 3, 18, 2, 90, 25, 3, 1, 1, 59642836)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (52, N'Oatmeal Raisin Crisp', N'G', N'C', 130, 3, 2, 170, 1.5, 13.5, 10, 120, 25, 3, 1.25, 0.5, 30450844)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (53, N'Post Nat. Raisin Bran', N'P', N'C', 120, 3, 1, 200, 6, 11, 14, 260, 25, 3, 1.33, 0.67, 3.784059E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (54, N'Product 19', N'K', N'C', 100, 3, 0, 320, 1, 20, 3, 45, 100, 3, 1, 1, 4.150354E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (55, N'Puffed Rice', N'Q', N'C', 50, 1, 0, 0, 0, 13, 0, 15, 0, 3, 0.5, 1, 6.075611E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (56, N'Puffed Wheat', N'Q', N'C', 50, 2, 0, 0, 1, 10, 0, 50, 0, 3, 0.5, 1, 63005644)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (57, N'Quaker Oat Squares', N'Q', N'C', 100, 4, 1, 135, 2, 14, 6, 110, 25, 3, 1, 0.5, 4.951187E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (58, N'Quaker Oatmeal', N'Q', N'H', 100, 5, 2, 0, 2.7, -1, -1, 110, 0, 1, 1, 0.67, 5.082839E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (59, N'Raisin Bran', N'K', N'C', 120, 3, 1, 210, 5, 14, 12, 240, 25, 2, 1.33, 0.75, 39259196)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (60, N'Raisin Nut Bran', N'G', N'C', 100, 3, 2, 140, 2.5, 10.5, 8, 140, 25, 3, 1, 0.5, 3.97034E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (61, N'Raisin Squares', N'K', N'C', 90, 2, 0, 0, 2, 15, 6, 110, 25, 3, 1, 0.5, 55333144)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (62, N'Rice Chex', N'R', N'C', 110, 1, 0, 240, 0, 23, 2, 30, 25, 1, 1, 1.13, 41998932)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (63, N'Rice Krispies', N'K', N'C', 110, 2, 0, 290, 0, 22, 3, 35, 25, 1, 1, 1, 4.056016E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (64, N'Shredded Wheat', N'N', N'C', 80, 2, 0, 0, 3, 16, 0, 95, 0, 1, 0.83, 1, 6.823589E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (65, N'Shredded Wheat ''n''Bran', N'N', N'C', 90, 3, 0, 0, 4, 19, 0, 140, 0, 1, 1, 0.67, 7.447295E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (66, N'Shredded Wheat spoon size', N'N', N'C', 90, 3, 0, 0, 3, 20, 0, 120, 0, 1, 1, 0.67, 72801784)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (67, N'Smacks', N'K', N'C', 110, 2, 1, 70, 1, 9, 15, 40, 25, 2, 1, 0.75, 31230054)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (68, N'Special K', N'K', N'C', 110, 6, 0, 230, 1, 16, 3, 55, 25, 1, 1, 1, 53131324)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (69, N'Strawberry Fruit Wheats', N'N', N'C', 90, 2, 0, 15, 3, 15, 5, 90, 25, 2, 1, 1, 5.936399E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (70, N'Total Corn Flakes', N'G', N'C', 110, 2, 1, 200, 0, 21, 3, 35, 100, 3, 1, 1, 38839744)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (71, N'Total Raisin Bran', N'G', N'C', 140, 3, 1, 190, 4, 15, 14, 230, 100, 3, 1.5, 1, 28592784)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (72, N'Total Whole Grain', N'G', N'C', 100, 3, 1, 200, 3, 16, 3, 110, 100, 3, 1, 1, 46658844)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (73, N'Triples', N'G', N'C', 110, 2, 1, 250, 0, 21, 3, 60, 25, 3, 1, 0.75, 39106176)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (74, N'Trix', N'G', N'C', 110, 1, 1, 140, 0, 13, 12, 25, 25, 2, 1, 1, 2.77533E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (75, N'Wheat Chex', N'R', N'C', 100, 3, 1, 230, 3, 17, 3, 115, 25, 1, 1, 0.67, 49787444)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (76, N'Wheaties', N'G', N'C', 100, 3, 1, 200, 3, 17, 3, 110, 25, 1, 1, 1, 5.159219E+07)
INSERT [dbo].[Cereals] ([id], [name], [mfr], [type], [calories], [protein], [fat], [sodium], [fiber], [carbo], [sugars], [potass], [vitamins], [shelf], [weight], [cups], [rating]) VALUES (80, N'Test Cereal', N'K', N'C', 110, 2, 0, 200, 1, 22, 3, 95, 25, 3, 1, 0.75, 42.5)
SET IDENTITY_INSERT [dbo].[Cereals] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Username], [Password], [Role]) VALUES (1, N'admin', N'$2a$11$e6QTyhWH6Yijkkc.2geEcuI7EJGFos3siTTuMBiCfovctXer1AHNi', N'Admin')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Cereals_name]    Script Date: 14-04-2025 10:30:36 ******/
ALTER TABLE [dbo].[Cereals] ADD  CONSTRAINT [UQ_Cereals_name] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__536C85E4ABEBAC96]    Script Date: 14-04-2025 10:30:36 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [Cereals] SET  READ_WRITE 
GO
