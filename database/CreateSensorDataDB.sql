SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE DATABASE AkkaNetResults
GO

CREATE DATABASE AkkaPersistence
GO

USE [AkkaNetResults]
GO

CREATE TABLE [dbo].[MeterReadings](
	[DeviceId] [uniqueidentifier] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[MeterReading] [decimal](18, 3) NOT NULL,
	[Consumption] [decimal](18, 3) NOT NULL,
	CONSTRAINT [PK_MeterReadings] PRIMARY KEY CLUSTERED 
	(
		[DeviceId] ASC,
		[Timestamp] DESC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
  ) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PeriodicAlertConfigurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [uniqueidentifier] NOT NULL,
	[NumberOfMinutes] [int] NOT NULL,
	[ThresholdConsumption] [decimal](18, 3) NOT NULL,
	CONSTRAINT [PK_PeriodicAlertConfigurations] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
  ) ON [PRIMARY]
GO

INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('3d5d044c-b389-44e5-b9fc-380d2514f780', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('4eea6097-985a-498c-8227-e64ce29d0a4a', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d1c58ce9-e1b5-4e3b-946e-8f37f78b02d0', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('2754fcfd-a3bd-4a73-b84b-50354bc9417a', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('93f15286-c054-42a5-8f44-5e0a4c2957c7', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('441d6dc2-33f3-4ee6-88f1-873910911e21', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('1c7e3a88-6b2c-4384-86e4-7cabe3807dc1', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('2e3101dc-75ac-40e3-a5b1-d18c7e1433e6', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('902f6fe8-99a1-4c0e-9a0c-c4918c068e43', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('2b7ecba0-35a7-4614-9b08-a4a1f67434fa', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('31220489-6ce0-4e0a-9b51-1d7df3c430af', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('5fa29c2b-bdc1-422b-954b-051761474302', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('bd670016-2cad-42f9-83d6-4aea960e3ecd', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('8a87f99c-2897-4f0f-8642-1e5bb7956bfc', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('1361e5ef-445b-4be1-a9cb-122ad2665d3e', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('3f264b0d-22f2-4d82-8261-38ce305d2af6', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('e9ea03d7-0421-42c2-a86e-654d2202461a', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('bc4111bd-8d27-4c14-ad5c-e18254dc591a', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('ea654beb-3718-46f3-881e-ba3272bff59c', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('03c17fa9-a4fd-409a-99fd-ee75ac10c9b3', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d2fb7bf3-0914-47dd-be4a-957fc41368a4', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('327f3a5b-8ac6-443c-9e97-47d67552418e', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d9d82046-a0d7-4b15-8ff6-d59fa9c5fbe4', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('c7cd70c9-cc39-4161-8e6f-6b9e7c7f9eca', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d9e790c9-b7ce-472f-822d-0edfc666fe9b', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('be9bda9a-bcbe-406c-b0b3-d921187a33e7', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('1396d019-4e51-4502-a894-a15610c99511', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('350e5f6a-b362-41ea-9c55-3cd0a52bb45f', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('4efeb68f-85d6-40db-8114-10a0e03ec682', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('03d00bd4-bd5f-406a-89ad-191523461125', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('e7155c07-ba36-4fe7-8f0e-870111575995', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('0a05fd5e-9b2e-4420-a443-4ec61b90ca91', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('07ac60c0-3667-42c8-85f7-acd564c47e98', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('054ca3cd-c0c7-49cd-94f1-730fc5cfb4b3', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('27809bcc-5d4a-4556-923a-83bc6f1ad0a9', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('5086affc-7a23-49d1-a106-b760fb468244', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('710a77f5-3abf-456b-a690-421fb2d9c241', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('46491247-6bc9-4fab-864f-894323740958', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('406d6e71-3876-4ec5-b4de-00b4b1c9ab52', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('f0804fa7-2f0f-4726-96d4-0fba78ea5be8', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('cbd8dfed-dcb3-45da-99e1-6177496f159f', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('74b2c935-365a-4f00-9d08-05096aa94acb', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('8acac431-b97e-4e8b-b57e-ec9addb8fc13', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('b403a9d7-a020-4dd2-9765-cd31343331a2', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('8a2215a0-937d-480b-80e6-90a34614e564', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('a0632ab2-e483-450a-b6cc-fce982868adf', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('06f2734e-06c9-4c3d-8321-435670562ab4', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d313b2d6-80fc-4cac-9b31-a9f42b22a768', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('9bea435a-5cd2-4c91-a46a-245509c67256', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('234f96ff-7181-453c-9ea0-fa636d7da46b', 20, 15);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('3d5d044c-b389-44e5-b9fc-380d2514f780', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('4eea6097-985a-498c-8227-e64ce29d0a4a', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d1c58ce9-e1b5-4e3b-946e-8f37f78b02d0', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('2754fcfd-a3bd-4a73-b84b-50354bc9417a', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('93f15286-c054-42a5-8f44-5e0a4c2957c7', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('441d6dc2-33f3-4ee6-88f1-873910911e21', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('1c7e3a88-6b2c-4384-86e4-7cabe3807dc1', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('2e3101dc-75ac-40e3-a5b1-d18c7e1433e6', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('902f6fe8-99a1-4c0e-9a0c-c4918c068e43', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('2b7ecba0-35a7-4614-9b08-a4a1f67434fa', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('31220489-6ce0-4e0a-9b51-1d7df3c430af', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('5fa29c2b-bdc1-422b-954b-051761474302', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('bd670016-2cad-42f9-83d6-4aea960e3ecd', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('8a87f99c-2897-4f0f-8642-1e5bb7956bfc', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('1361e5ef-445b-4be1-a9cb-122ad2665d3e', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('3f264b0d-22f2-4d82-8261-38ce305d2af6', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('e9ea03d7-0421-42c2-a86e-654d2202461a', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('bc4111bd-8d27-4c14-ad5c-e18254dc591a', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('ea654beb-3718-46f3-881e-ba3272bff59c', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('03c17fa9-a4fd-409a-99fd-ee75ac10c9b3', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d2fb7bf3-0914-47dd-be4a-957fc41368a4', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('327f3a5b-8ac6-443c-9e97-47d67552418e', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d9d82046-a0d7-4b15-8ff6-d59fa9c5fbe4', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('c7cd70c9-cc39-4161-8e6f-6b9e7c7f9eca', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d9e790c9-b7ce-472f-822d-0edfc666fe9b', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('be9bda9a-bcbe-406c-b0b3-d921187a33e7', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('1396d019-4e51-4502-a894-a15610c99511', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('350e5f6a-b362-41ea-9c55-3cd0a52bb45f', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('4efeb68f-85d6-40db-8114-10a0e03ec682', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('03d00bd4-bd5f-406a-89ad-191523461125', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('e7155c07-ba36-4fe7-8f0e-870111575995', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('0a05fd5e-9b2e-4420-a443-4ec61b90ca91', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('07ac60c0-3667-42c8-85f7-acd564c47e98', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('054ca3cd-c0c7-49cd-94f1-730fc5cfb4b3', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('27809bcc-5d4a-4556-923a-83bc6f1ad0a9', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('5086affc-7a23-49d1-a106-b760fb468244', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('710a77f5-3abf-456b-a690-421fb2d9c241', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('46491247-6bc9-4fab-864f-894323740958', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('406d6e71-3876-4ec5-b4de-00b4b1c9ab52', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('f0804fa7-2f0f-4726-96d4-0fba78ea5be8', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('cbd8dfed-dcb3-45da-99e1-6177496f159f', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('74b2c935-365a-4f00-9d08-05096aa94acb', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('8acac431-b97e-4e8b-b57e-ec9addb8fc13', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('b403a9d7-a020-4dd2-9765-cd31343331a2', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('8a2215a0-937d-480b-80e6-90a34614e564', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('a0632ab2-e483-450a-b6cc-fce982868adf', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('06f2734e-06c9-4c3d-8321-435670562ab4', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('d313b2d6-80fc-4cac-9b31-a9f42b22a768', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('9bea435a-5cd2-4c91-a46a-245509c67256', 5, 20);
INSERT INTO [dbo].[PeriodicAlertConfigurations] ([DeviceId], [NumberOfMinutes], [ThresholdConsumption]) VALUES ('234f96ff-7181-453c-9ea0-fa636d7da46b', 5, 20);
GO