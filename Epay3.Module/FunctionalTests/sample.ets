;#DropDB Epay3EasyTest

#Application Epay3.Win
#Application Epay3.Web

*FillForm
 User Name = Admin
*Action Log On

#IfDef Epay3.Win

*Action About...
 
*Action OK

#EndIf

#IfDef Epay3.Web
*Action Navigation(My Details)
#EndIf
