namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmEntities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClientName = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        Risk = c.Int(nullable: false),
                        ServiceId = c.String(nullable: false),
                        TimeOfAlarm = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AlarmEntities");
        }
    }
}
