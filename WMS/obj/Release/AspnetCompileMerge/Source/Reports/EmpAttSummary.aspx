﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="EmpAttSummary.aspx.cs" Inherits="WMS.Reports.EmpAttSummary" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/modernizr-2.6.2.js"></script>
    <script src="../Scripts/jquery-2.1.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="report-container">
         <div class="report-filter">
             <div style="height:20px;"></div>
             <div class="button-divDate">
                 <span style="color:whitesmoke; font-size:15px;">From: </span><input id="dateFrom" type="date" runat="server" style="height:30px;" />
                 <span style="color:whitesmoke; font-size:15px;">To: </span><input id="dateTo" type="date" runat="server" style="height:30px;" />
             </div>
            <div class="button-div">
                <asp:Button ID="btnGenerateReport" CssClass="btn btn-success" 
                    runat="server" Text="Generate Report" onclick="btnGenerateReport_Click" Width="190px" />

            </div>
            <div class="button-div"><asp:Button ID="btnEmployeeGrid" CssClass="btn btn-primary" runat="server" Text="Select Employee" onclick="btnEmployeeGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbEmpCount" runat="server" Text="Selected Employees: 0" Font-Bold="False" Font-Italic="True"  Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
             </div>

            <div class="button-div"><asp:Button ID="btnSectionGrid" CssClass="btn btn-primary" runat="server" Text="Select Section" onclick="btnSectionGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbSectionCount" runat="server" Text="Selected Sections: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div"><asp:Button ID="btnDepartmentGrid" CssClass="btn btn-primary" runat="server" 
                Text="Select Department" onclick="btnDepartmentGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbDeptCount" runat="server" Text="Selected Departments: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div"><asp:Button ID="btnCrewGrid" CssClass="btn btn-primary" runat="server" 
                Text="Select Crew" onclick="btnCrewGrid_Click" Width="180px" />
                <div><asp:Label ID="lbCrewCount" runat="server" Text="Selected Crews: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div"><asp:Button ID="btnShiftGrid" CssClass="btn btn-primary" runat="server" 
                Text="Select Shift" onclick="btnShiftGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbShiftCount" runat="server" Text="Selected Shifts: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div">
                <asp:Button ID="btnLoc" CssClass="btn btn-primary" runat="server" 
                Text="Select Locations" onclick="btnLoc_Click" Width="180px"/>
                <div><asp:Label ID="lbLocCount" runat="server" Text="Selected Locations: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div">
                <asp:Button ID="btnEmpType" CssClass="btn btn-primary" runat="server" 
                Text="Select Employee Type" onclick="btnEmpType_Click" Width="180px"/>
                <div><asp:Label ID="lbCatCount" runat="server" Text="Selected Types: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>


            <div class="button-div"></div>

         </div> <%--End div Report-filter--%>

         <div class="report-viewer-container" style="margin: 0 auto;">
             <%-- Emp Grid --%>
             <div runat="server" id="DivGridEmp" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Employee </div>
             <asp:GridView ID="grid_Employee" runat="server" OnRowDataBound="grid_Employee_RowDataBound" AutoGenerateColumns="False" DataKeyNames="EmpID" DataSourceID="ObjectDataSource1">
                 <Columns>
                      <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlEmp" runat="server" oncheckedchanged="chkCtrlEmp_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:BoundField DataField="EmpID" HeaderText="EmpID"
                         SortExpression ="EmpID" />
                     <asp:BoundField DataField="EmpNo" HeaderText="EmpNo" SortExpression="EmpNo" />
                     <asp:BoundField DataField="EmpName" HeaderText="EmpName" SortExpression="EmpName" />
                      <asp:BoundField DataField="DesignationName" HeaderText="DesignationName" SortExpression="DesignationName" />
                     <asp:BoundField DataField="CardNo" HeaderText="CardNo" SortExpression="CardNo" />
                     <asp:BoundField DataField="CrewName" HeaderText="CrewName" SortExpression="CrewName" />
                     <asp:BoundField DataField="TypeName" HeaderText="TypeName" SortExpression="TypeName" />
                     <asp:BoundField DataField="CatName" HeaderText="CatName" SortExpression="CatName" />
                     <asp:BoundField DataField="ShiftName" HeaderText="ShiftName" SortExpression="ShiftName" />
                     <asp:BoundField DataField="SectionName" HeaderText="SectionName" SortExpression="SectionName" />
                     
                 </Columns>
             </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.EmpViewTableAdapter"></asp:ObjectDataSource>
            </div>

             <%-- Section Grid --%>
             <div runat="server" id="DivGridSection" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Section </div>
             <asp:GridView ID="grid_Section" runat="server" AutoGenerateColumns="False" DataKeyNames="SectionID" DataSourceID="ObjectDataSource2">
                 <Columns>
                     <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlSection" runat="server" oncheckedchanged="chkCtrlSection_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:BoundField DataField="SectionID" HeaderText="SectionID" InsertVisible="False" ReadOnly="True" SortExpression="SectionID" />
                     <asp:BoundField DataField="SectionName" HeaderText="SectionName" SortExpression="SectionName" />
                     <asp:BoundField DataField="DeptID" HeaderText="DeptID" SortExpression="DeptID" />
                 </Columns>
             </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.SectionTableAdapter">
                 </asp:ObjectDataSource>
             </div>

             <%-- Department Grid --%>
             <div runat="server" id="DivGridDept" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Department </div>
                 <asp:GridView ID="grid_Dept" runat="server" AutoGenerateColumns="False" DataKeyNames="DeptID" DataSourceID="ObjectDataSource3">
                     <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlDept" runat="server" oncheckedchanged="chkCtrlDept_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="DeptID" HeaderText="DeptID" InsertVisible="False" ReadOnly="True" SortExpression="DeptID" />
                         <asp:BoundField DataField="DeptName" HeaderText="DeptName" SortExpression="DeptName" />
                         <asp:BoundField DataField="DivID" HeaderText="DivID" SortExpression="DivID" />
                     </Columns>
                 </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource3" runat="server"  OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.DepartmentTableAdapter">
                 </asp:ObjectDataSource>
             </div>

             <%-- Location Grid --%>
             <div runat="server" id="DivLocGrid" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Location </div>
                 <asp:GridView ID="grid_Location" runat="server" AutoGenerateColumns="False" DataKeyNames="LocID" DataSourceID="ObjectDataSource4">
                     <Columns>
                         <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCtrlLoc" runat="server"  oncheckedchanged="chkCtrlLoc_CheckedChanged"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <asp:BoundField DataField="LocID" HeaderText="LocID" InsertVisible="False" ReadOnly="True" SortExpression="LocID" />
                         <asp:BoundField DataField="LocName" HeaderText="LocName" SortExpression="LocName" />
                     </Columns>
                 </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource4" runat="server"  OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.LocationTableAdapter">
                 </asp:ObjectDataSource>
             </div>

             <%-- Crew Grid --%>
             <div runat="server" id="DivGridCrew" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Crew </div>
                 <asp:GridView ID="grid_Crew" runat="server" AutoGenerateColumns="False" DataKeyNames="CrewID" DataSourceID="ObjectDataSource5">
                     <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlCrew" runat="server"  oncheckedchanged="chkCtrlCrew_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="CrewID" HeaderText="CrewID" InsertVisible="False" ReadOnly="True" SortExpression="CrewID" />
                         <asp:BoundField DataField="CrewName" HeaderText="CrewName" SortExpression="CrewName" />
                     </Columns>
                 </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource5" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.CrewTableAdapter">
                 </asp:ObjectDataSource>
             </div>

             <%-- EmpType Grid --%>
             <div runat="server" id="DivTypeGrid" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Type </div>
                 <asp:GridView ID="grid_EmpType" runat="server" AutoGenerateColumns="False" DataKeyNames="TypeID" DataSourceID="ObjectDataSource6">
                     <Columns>
                         <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCtrlType" runat="server"  oncheckedchanged="chkCtrlType_CheckedChanged"/>
                                </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="TypeID" HeaderText="TypeID" ReadOnly="True" SortExpression="TypeID" />
                         <asp:BoundField DataField="TypeName" HeaderText="TypeName" SortExpression="TypeName" />
                         <asp:BoundField DataField="CatID" HeaderText="CatID" SortExpression="CatID" />
                     </Columns>
                 </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource6" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.EmpTypeTableAdapter" >
                 </asp:ObjectDataSource>
             </div>

             <%-- Shift Grid --%>
             <div runat="server" id="DivShiftGrid" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Shift </div>
                 <asp:GridView ID="grid_Shift" runat="server" AutoGenerateColumns="False" DataKeyNames="ShiftID" DataSourceID="ObjectDataSource7">
                     <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlShift" runat="server"  oncheckedchanged="chkCtrlShift_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="ShiftID" HeaderText="ShiftID" ReadOnly="True" SortExpression="ShiftID" />
                         <asp:BoundField DataField="ShiftName" HeaderText="ShiftName" SortExpression="ShiftName" />
                         <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" />
                     </Columns>
                 </asp:GridView>
                 <asp:ObjectDataSource ID="ObjectDataSource7" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.ShiftTableAdapter" >
                 </asp:ObjectDataSource>
             </div>
             <div>
               
                 <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                     <LocalReport ReportPath="Reports\RDLC\EmpAttSummary.rdlc">
                         <DataSources>
                             <rsweb:ReportDataSource DataSourceId="ObjectDataSource8" Name="DataSet1" />
                         </DataSources>
                     </LocalReport>
                 </rsweb:ReportViewer>
                 <asp:ObjectDataSource ID="ObjectDataSource8" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.ViewSummaryTableAdapter"></asp:ObjectDataSource>
                 <asp:ScriptManager ID="ScriptManager1" runat="server">
                 </asp:ScriptManager>
             </div>
         </div> <%--End div Report-viewer-container--%>
         <div class="clearfix">
             
         </div> 
    </div><%--End div Report-container--%>
</asp:Content>


