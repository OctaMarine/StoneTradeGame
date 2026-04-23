StoneTradeGame

cd StoneActionServer.WebApi
dotnet run --launch-profile http


-------------------

dotnet ef migrations add InitMigration --project StoneActionServer.DAL
dotnet ef database update --project StoneActionServer.DAL

token  
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyTmFtZSI6InJyciIsImVtYWlsIjoicnJyIiwiaWQiOiIxIiwiZXhwIjoxNzc2OTY2MzM0fQ.FlUVCDunhM1-DAOTp_XRTuoDXaleEUdp2GXmowZ2ESE