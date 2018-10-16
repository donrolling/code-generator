declare @entityId bigint = 0
INSERT INTO [dbo].[Entity] ([ProjectId],[Name]) VALUES(1, 'Model.Application.Name')
set @entityId = SCOPE_IDENTITY()
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 1, (select top 1 Id from DataType where DotNetEnumValue = 23), 0, 16)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 14), 0, 500)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 14), 0, 500)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 14), 0, 500)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 3), 0, 1)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 6), 1, 8)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 23), 1, 16)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 6), 1, 8)
Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, 'Model.Application.Name', 0, (select top 1 Id from DataType where DotNetEnumValue = 23), 1, 16)

--****************************************************

