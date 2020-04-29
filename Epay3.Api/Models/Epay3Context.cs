using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Epay3.Api.Models
{
    public partial class Epay3Context : DbContext
    {
        public Epay3Context()
        {
        }


        public virtual DbSet<Analysis> Analysis { get; set; }
        public virtual DbSet<AuditDataItemPersistent> AuditDataItemPersistent { get; set; }
        public virtual DbSet<AuditedObjectWeakReference> AuditedObjectWeakReference { get; set; }
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<Calendar> Calendar { get; set; }
        public virtual DbSet<CalendarDay> CalendarDay { get; set; }
        public virtual DbSet<CalendarDayCalendarDaysCalendarCalendars> CalendarDayCalendarDaysCalendarCalendars { get; set; }
        public virtual DbSet<Card> Card { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DashboardData> DashboardData { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceLogin> DeviceLogin { get; set; }
        public virtual DbSet<DeviceRegistration> DeviceRegistration { get; set; }
        public virtual DbSet<ItemPermission> ItemPermission { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LocationCard> LocationCard { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<ModelDifference> ModelDifference { get; set; }
        public virtual DbSet<ModelDifferenceAspect> ModelDifferenceAspect { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<PermissionPolicyMemberPermissionsObject> PermissionPolicyMemberPermissionsObject { get; set; }
        public virtual DbSet<PermissionPolicyNavigationPermissionsObject> PermissionPolicyNavigationPermissionsObject { get; set; }
        public virtual DbSet<PermissionPolicyObjectPermissionsObject> PermissionPolicyObjectPermissionsObject { get; set; }
        public virtual DbSet<PermissionPolicyRole> PermissionPolicyRole { get; set; }
        public virtual DbSet<PermissionPolicyTypePermissionsObject> PermissionPolicyTypePermissionsObject { get; set; }
        public virtual DbSet<PermissionPolicyUser> PermissionPolicyUser { get; set; }
        public virtual DbSet<PermissionPolicyUserUsersPermissionPolicyRoleRoles> PermissionPolicyUserUsersPermissionPolicyRoleRoles { get; set; }
        public virtual DbSet<RegistrationRequest> RegistrationRequest { get; set; }
        public virtual DbSet<ReportDataV2> ReportDataV2 { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<StationItem> StationItem { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionDetail> TransactionDetail { get; set; }
        public virtual DbSet<TransactionItem> TransactionItem { get; set; }
        public virtual DbSet<TransactionItemCategory> TransactionItemCategory { get; set; }
        public virtual DbSet<XpobjectType> XpobjectType { get; set; }
        public virtual DbSet<XpweakReference> XpweakReference { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Analysis>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_Analysis");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<AuditDataItemPersistent>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.AuditedObject)
                    .HasName("iAuditedObject_AuditDataItemPersistent");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_AuditDataItemPersistent");

                entity.HasIndex(e => e.ModifiedOn)
                    .HasName("iModifiedOn_AuditDataItemPersistent");

                entity.HasIndex(e => e.NewObject)
                    .HasName("iNewObject_AuditDataItemPersistent");

                entity.HasIndex(e => e.OldObject)
                    .HasName("iOldObject_AuditDataItemPersistent");

                entity.HasIndex(e => e.OperationType)
                    .HasName("iOperationType_AuditDataItemPersistent");

                entity.HasIndex(e => e.UserName)
                    .HasName("iUserName_AuditDataItemPersistent");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(2048);

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.NewValue).HasMaxLength(1024);

                entity.Property(e => e.OldValue).HasMaxLength(1024);

                entity.Property(e => e.OperationType).HasMaxLength(100);

                entity.Property(e => e.PropertyName).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.AuditedObjectNavigation)
                    .WithMany(p => p.AuditDataItemPersistent)
                    .HasForeignKey(d => d.AuditedObject)
                    .HasConstraintName("FK_AuditDataItemPersistent_AuditedObject");

                entity.HasOne(d => d.NewObjectNavigation)
                    .WithMany(p => p.AuditDataItemPersistentNewObjectNavigation)
                    .HasForeignKey(d => d.NewObject)
                    .HasConstraintName("FK_AuditDataItemPersistent_NewObject");

                entity.HasOne(d => d.OldObjectNavigation)
                    .WithMany(p => p.AuditDataItemPersistentOldObjectNavigation)
                    .HasForeignKey(d => d.OldObject)
                    .HasConstraintName("FK_AuditDataItemPersistent_OldObject");
            });

            modelBuilder.Entity<AuditedObjectWeakReference>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.DisplayName).HasMaxLength(250);

                entity.HasOne(d => d.O)
                    .WithOne(p => p.AuditedObjectWeakReference)
                    .HasForeignKey<AuditedObjectWeakReference>(d => d.Oid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuditedObjectWeakReference_Oid");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<CalendarDay>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Date).HasColumnType("datetime");
            });

            modelBuilder.Entity<CalendarDayCalendarDaysCalendarCalendars>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.ToTable("CalendarDayCalendarDays_CalendarCalendars");

                entity.HasIndex(e => e.CalendarDays)
                    .HasName("iCalendarDays_CalendarDayCalendarDays_CalendarCalendars");

                entity.HasIndex(e => e.Calendars)
                    .HasName("iCalendars_CalendarDayCalendarDays_CalendarCalendars");

                entity.HasIndex(e => new { e.Calendars, e.CalendarDays })
                    .HasName("iCalendarsCalendarDays_CalendarDayCalendarDays_CalendarCalendars")
                    .IsUnique();

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.HasOne(d => d.CalendarDaysNavigation)
                    .WithMany(p => p.CalendarDayCalendarDaysCalendarCalendars)
                    .HasForeignKey(d => d.CalendarDays)
                    .HasConstraintName("FK_CalendarDayCalendarDays_CalendarCalendars_CalendarDays");

                entity.HasOne(d => d.CalendarsNavigation)
                    .WithMany(p => p.CalendarDayCalendarDaysCalendarCalendars)
                    .HasForeignKey(d => d.Calendars)
                    .HasConstraintName("FK_CalendarDayCalendarDays_CalendarCalendars_Calendars");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.CardNo)
                    .HasName("iCardNo_Card")
                    .IsUnique();

                entity.HasIndex(e => e.Customer)
                    .HasName("iCustomer_Card");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CardNo).HasMaxLength(100);

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.Card)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK_Card_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.ObjectType)
                    .HasName("iObjectType_Customer");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.CitizenshipNumber).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.NotificationToken).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.Team).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(100);

                entity.HasOne(d => d.ObjectTypeNavigation)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.ObjectType)
                    .HasConstraintName("FK_Customer_ObjectType");
            });

            modelBuilder.Entity<DashboardData>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_DashboardData");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.DeviceToken).HasMaxLength(100);

                entity.Property(e => e.Serial).HasMaxLength(100);
            });

            modelBuilder.Entity<DeviceLogin>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Device)
                    .HasName("iDevice_DeviceLogin");

                entity.HasIndex(e => e.Employee)
                    .HasName("iEmployee_DeviceLogin");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.PasswordReceived).HasMaxLength(100);

                entity.Property(e => e.Serial).HasMaxLength(100);

                entity.Property(e => e.SourceAddress).HasMaxLength(100);

                entity.Property(e => e.Success).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(100);

                entity.Property(e => e.UserNameReceived).HasMaxLength(100);

                entity.HasOne(d => d.DeviceNavigation)
                    .WithMany(p => p.DeviceLogin)
                    .HasForeignKey(d => d.Device)
                    .HasConstraintName("FK_DeviceLogin_Device");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.DeviceLogin)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_DeviceLogin_Employee");
            });

            modelBuilder.Entity<DeviceRegistration>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_DeviceRegistration");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Serial).HasMaxLength(100);

                entity.Property(e => e.SourceAddress).HasMaxLength(100);

                entity.Property(e => e.Token).HasMaxLength(100);
            });

            modelBuilder.Entity<ItemPermission>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Customer)
                    .HasName("iCustomer_ItemPermission");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_ItemPermission");

                entity.HasIndex(e => e.TransactionItem)
                    .HasName("iTransactionItem_ItemPermission");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.ItemPermission)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK_ItemPermission_Customer");

                entity.HasOne(d => d.TransactionItemNavigation)
                    .WithMany(p => p.ItemPermission)
                    .HasForeignKey(d => d.TransactionItem)
                    .HasConstraintName("FK_ItemPermission_TransactionItem");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Customer)
                    .HasName("iCustomer_Location");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_Location");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK_Location_Customer");
            });

            modelBuilder.Entity<LocationCard>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Card)
                    .HasName("iCard_LocationCard");

                entity.HasIndex(e => e.DeviceLogin)
                    .HasName("iDeviceLogin_LocationCard");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_LocationCard");

                entity.HasIndex(e => e.Location)
                    .HasName("iLocation_LocationCard");

                entity.HasIndex(e => e.OpenedBy)
                    .HasName("iOpenedBy_LocationCard");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.ClosedAt).HasColumnType("datetime");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.OpenedAt).HasColumnType("datetime");

                entity.HasOne(d => d.CardNavigation)
                    .WithMany(p => p.LocationCard)
                    .HasForeignKey(d => d.Card)
                    .HasConstraintName("FK_LocationCard_Card");

                entity.HasOne(d => d.DeviceLoginNavigation)
                    .WithMany(p => p.LocationCard)
                    .HasForeignKey(d => d.DeviceLogin)
                    .HasConstraintName("FK_LocationCard_DeviceLogin");

                entity.HasOne(d => d.LocationNavigation)
                    .WithMany(p => p.LocationCard)
                    .HasForeignKey(d => d.Location)
                    .HasConstraintName("FK_LocationCard_Location");

                entity.HasOne(d => d.OpenedByNavigation)
                    .WithMany(p => p.LocationCard)
                    .HasForeignKey(d => d.OpenedBy)
                    .HasConstraintName("FK_LocationCard_OpenedBy");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_Message");

                entity.HasIndex(e => e.To)
                    .HasName("iTo_Message");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Content).HasMaxLength(100);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.ToNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.To)
                    .HasConstraintName("FK_Message_To");
            });

            modelBuilder.Entity<ModelDifference>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_ModelDifference");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.ContextId).HasMaxLength(100);

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.UserId).HasMaxLength(100);
            });

            modelBuilder.Entity<ModelDifferenceAspect>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_ModelDifferenceAspect");

                entity.HasIndex(e => e.Owner)
                    .HasName("iOwner_ModelDifferenceAspect");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.ModelDifferenceAspect)
                    .HasForeignKey(d => d.Owner)
                    .HasConstraintName("FK_ModelDifferenceAspect_Owner");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Customer)
                    .HasName("iCustomer_Order");

                entity.HasIndex(e => e.LocationCard)
                    .HasName("iLocationCard_Order");

                entity.HasIndex(e => e.Transaction)
                    .HasName("iTransaction_Order");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.LocationCardNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.LocationCard)
                    .HasConstraintName("FK_Order_LocationCard");

                entity.HasOne(d => d.TransactionNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.Transaction)
                    .HasConstraintName("FK_Order_Transaction");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.CancelTransactionDetail)
                    .HasName("iCancelTransactionDetail_OrderDetail");

                entity.HasIndex(e => e.Order)
                    .HasName("iOrder_OrderDetail");

                entity.HasIndex(e => e.Station)
                    .HasName("iStation_OrderDetail");

                entity.HasIndex(e => e.TransactionDetail)
                    .HasName("iTransactionDetail_OrderDetail");

                entity.HasIndex(e => e.TransactionItem)
                    .HasName("iTransactionItem_OrderDetail");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Quantity).HasColumnType("money");

                entity.HasOne(d => d.CancelTransactionDetailNavigation)
                    .WithMany(p => p.OrderDetailCancelTransactionDetailNavigation)
                    .HasForeignKey(d => d.CancelTransactionDetail)
                    .HasConstraintName("FK_OrderDetail_CancelTransactionDetail");

                entity.HasOne(d => d.OrderNavigation)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.Order)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.StationNavigation)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.Station)
                    .HasConstraintName("FK_OrderDetail_Station");

                entity.HasOne(d => d.TransactionDetailNavigation)
                    .WithMany(p => p.OrderDetailTransactionDetailNavigation)
                    .HasForeignKey(d => d.TransactionDetail)
                    .HasConstraintName("FK_OrderDetail_TransactionDetail");

                entity.HasOne(d => d.TransactionItemNavigation)
                    .WithMany(p => p.OrderDetail)
                    .HasForeignKey(d => d.TransactionItem)
                    .HasConstraintName("FK_OrderDetail_TransactionItem");
            });

            modelBuilder.Entity<PermissionPolicyMemberPermissionsObject>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_PermissionPolicyMemberPermissionsObject");

                entity.HasIndex(e => e.TypePermissionObject)
                    .HasName("iTypePermissionObject_PermissionPolicyMemberPermissionsObject");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.HasOne(d => d.TypePermissionObjectNavigation)
                    .WithMany(p => p.PermissionPolicyMemberPermissionsObject)
                    .HasForeignKey(d => d.TypePermissionObject)
                    .HasConstraintName("FK_PermissionPolicyMemberPermissionsObject_TypePermissionObject");
            });

            modelBuilder.Entity<PermissionPolicyNavigationPermissionsObject>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_PermissionPolicyNavigationPermissionsObject");

                entity.HasIndex(e => e.Role)
                    .HasName("iRole_PermissionPolicyNavigationPermissionsObject");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.PermissionPolicyNavigationPermissionsObject)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_PermissionPolicyNavigationPermissionsObject_Role");
            });

            modelBuilder.Entity<PermissionPolicyObjectPermissionsObject>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_PermissionPolicyObjectPermissionsObject");

                entity.HasIndex(e => e.TypePermissionObject)
                    .HasName("iTypePermissionObject_PermissionPolicyObjectPermissionsObject");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.HasOne(d => d.TypePermissionObjectNavigation)
                    .WithMany(p => p.PermissionPolicyObjectPermissionsObject)
                    .HasForeignKey(d => d.TypePermissionObject)
                    .HasConstraintName("FK_PermissionPolicyObjectPermissionsObject_TypePermissionObject");
            });

            modelBuilder.Entity<PermissionPolicyRole>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_PermissionPolicyRole");

                entity.HasIndex(e => e.ObjectType)
                    .HasName("iObjectType_PermissionPolicyRole");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.ObjectTypeNavigation)
                    .WithMany(p => p.PermissionPolicyRole)
                    .HasForeignKey(d => d.ObjectType)
                    .HasConstraintName("FK_PermissionPolicyRole_ObjectType");
            });

            modelBuilder.Entity<PermissionPolicyTypePermissionsObject>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_PermissionPolicyTypePermissionsObject");

                entity.HasIndex(e => e.Role)
                    .HasName("iRole_PermissionPolicyTypePermissionsObject");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.PermissionPolicyTypePermissionsObject)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_PermissionPolicyTypePermissionsObject_Role");
            });

            modelBuilder.Entity<PermissionPolicyUser>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_PermissionPolicyUser");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<PermissionPolicyUserUsersPermissionPolicyRoleRoles>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.ToTable("PermissionPolicyUserUsers_PermissionPolicyRoleRoles");

                entity.HasIndex(e => e.Roles)
                    .HasName("iRoles_PermissionPolicyUserUsers_PermissionPolicyRoleRoles");

                entity.HasIndex(e => e.Users)
                    .HasName("iUsers_PermissionPolicyUserUsers_PermissionPolicyRoleRoles");

                entity.HasIndex(e => new { e.Roles, e.Users })
                    .HasName("iRolesUsers_PermissionPolicyUserUsers_PermissionPolicyRoleRoles")
                    .IsUnique();

                entity.Property(e => e.Oid)
                    .HasColumnName("OID")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.RolesNavigation)
                    .WithMany(p => p.PermissionPolicyUserUsersPermissionPolicyRoleRoles)
                    .HasForeignKey(d => d.Roles)
                    .HasConstraintName("FK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles_Roles");

                entity.HasOne(d => d.UsersNavigation)
                    .WithMany(p => p.PermissionPolicyUserUsersPermissionPolicyRoleRoles)
                    .HasForeignKey(d => d.Users)
                    .HasConstraintName("FK_PermissionPolicyUserUsers_PermissionPolicyRoleRoles_Users");
            });

            modelBuilder.Entity<RegistrationRequest>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Customer)
                    .HasName("iCustomer_RegistrationRequest");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_RegistrationRequest");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.DateConsumed).HasColumnType("datetime");

                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.IpAddress).HasMaxLength(100);

                entity.Property(e => e.VerificationCode).HasMaxLength(100);

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.RegistrationRequest)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("FK_RegistrationRequest_Customer");
            });

            modelBuilder.Entity<ReportDataV2>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_ReportDataV2");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ObjectTypeName).HasMaxLength(512);

                entity.Property(e => e.ParametersObjectTypeName).HasMaxLength(512);

                entity.Property(e => e.PredefinedReportType).HasMaxLength(512);
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Latitude).HasColumnType("money");

                entity.Property(e => e.Longitude).HasColumnType("money");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.RegistrationCode).HasMaxLength(100);

                entity.Property(e => e.TransactionFee).HasColumnType("money");

                entity.Property(e => e.TransactionMonthlyLimit).HasColumnType("money");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Employee)
                    .HasName("iEmployee_Station");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_Station");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Station)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_Station_Employee");
            });

            modelBuilder.Entity<StationItem>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_StationItem");

                entity.HasIndex(e => e.Station)
                    .HasName("iStation_StationItem");

                entity.HasIndex(e => e.TransactionItem)
                    .HasName("iTransactionItem_StationItem");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.HasOne(d => d.StationNavigation)
                    .WithMany(p => p.StationItem)
                    .HasForeignKey(d => d.Station)
                    .HasConstraintName("FK_StationItem_Station");

                entity.HasOne(d => d.TransactionItemNavigation)
                    .WithMany(p => p.StationItem)
                    .HasForeignKey(d => d.TransactionItem)
                    .HasConstraintName("FK_StationItem_TransactionItem");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Card)
                    .HasName("iCard_Transaction");

                entity.HasIndex(e => e.Device)
                    .HasName("iDevice_Transaction");

                entity.HasIndex(e => e.DeviceLogin)
                    .HasName("iDeviceLogin_Transaction");

                entity.HasIndex(e => e.Employee)
                    .HasName("iEmployee_Transaction");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Amount).HasColumnType("money");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.DateEffective).HasColumnType("datetime");

                entity.Property(e => e.NewBalance).HasColumnType("money");

                entity.Property(e => e.OldBalance).HasColumnType("money");

                entity.Property(e => e.ProvisionDate).HasColumnType("datetime");

                entity.Property(e => e.SourceAddress).HasMaxLength(100);

                entity.Property(e => e.TotalCharges).HasColumnType("money");

                entity.Property(e => e.TotalSales).HasColumnType("money");

                entity.HasOne(d => d.CardNavigation)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.Card)
                    .HasConstraintName("FK_Transaction_Card");

                entity.HasOne(d => d.DeviceNavigation)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.Device)
                    .HasConstraintName("FK_Transaction_Device");

                entity.HasOne(d => d.DeviceLoginNavigation)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.DeviceLogin)
                    .HasConstraintName("FK_Transaction_DeviceLogin");

                entity.HasOne(d => d.EmployeeNavigation)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.Employee)
                    .HasConstraintName("FK_Transaction_Employee");
            });

            modelBuilder.Entity<TransactionDetail>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Item)
                    .HasName("iItem_TransactionDetail");

                entity.HasIndex(e => e.Transaction)
                    .HasName("iTransaction_TransactionDetail");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Quantity).HasColumnType("money");

                entity.Property(e => e.Total).HasColumnType("money");

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTo).HasColumnType("datetime");

                entity.HasOne(d => d.ItemNavigation)
                    .WithMany(p => p.TransactionDetail)
                    .HasForeignKey(d => d.Item)
                    .HasConstraintName("FK_TransactionDetail_Item");

                entity.HasOne(d => d.TransactionNavigation)
                    .WithMany(p => p.TransactionDetail)
                    .HasForeignKey(d => d.Transaction)
                    .HasConstraintName("FK_TransactionDetail_Transaction");
            });

            modelBuilder.Entity<TransactionItem>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Category)
                    .HasName("iCategory_TransactionItem");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.SpecialCode).HasMaxLength(100);

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.TransactionItem)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FK_TransactionItem_Category");
            });

            modelBuilder.Entity<TransactionItemCategory>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_TransactionItemCategory");

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.Icon).HasMaxLength(100);

                entity.Property(e => e.ImageUrl).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Order).HasMaxLength(100);
            });

            modelBuilder.Entity<XpobjectType>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.ToTable("XPObjectType");

                entity.HasIndex(e => e.TypeName)
                    .HasName("iTypeName_XPObjectType")
                    .IsUnique();

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.AssemblyName).HasMaxLength(254);

                entity.Property(e => e.TypeName).HasMaxLength(254);
            });

            modelBuilder.Entity<XpweakReference>(entity =>
            {
                entity.HasKey(e => e.Oid);

                entity.ToTable("XPWeakReference");

                entity.HasIndex(e => e.Gcrecord)
                    .HasName("iGCRecord_XPWeakReference");

                entity.HasIndex(e => e.ObjectType)
                    .HasName("iObjectType_XPWeakReference");

                entity.HasIndex(e => e.TargetType)
                    .HasName("iTargetType_XPWeakReference");

                entity.Property(e => e.Oid).ValueGeneratedNever();

                entity.Property(e => e.Gcrecord).HasColumnName("GCRecord");

                entity.Property(e => e.TargetKey).HasMaxLength(100);

                entity.HasOne(d => d.ObjectTypeNavigation)
                    .WithMany(p => p.XpweakReferenceObjectTypeNavigation)
                    .HasForeignKey(d => d.ObjectType)
                    .HasConstraintName("FK_XPWeakReference_ObjectType");

                entity.HasOne(d => d.TargetTypeNavigation)
                    .WithMany(p => p.XpweakReferenceTargetTypeNavigation)
                    .HasForeignKey(d => d.TargetType)
                    .HasConstraintName("FK_XPWeakReference_TargetType");
            });
        }
    }
}
