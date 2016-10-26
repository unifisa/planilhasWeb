﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="relatorio.aspx.cs" Inherits="Planilhas.relatorio" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   
        <form id="form1" runat="server">

        <asp:ObjectDataSource ID="Executar" runat="server" SelectMethod="GetData" TypeName="Planilhas.PlanilhasDBDataSetTableAdapters.RequisicaosTableAdapter" DeleteMethod="Delete" InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" UpdateMethod="Update">
            <DeleteParameters>
                <asp:Parameter Name="Original_ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="empresa" Type="String" />
                <asp:Parameter Name="data" Type="DateTime" />
                <asp:Parameter Name="numero" Type="Int32" />
                <asp:Parameter Name="requisitante" Type="String" />
                <asp:Parameter Name="unidade" Type="String" />
                <asp:Parameter Name="departamento" Type="String" />
                <asp:Parameter Name="descricao_desp1" Type="String" />
                <asp:Parameter Name="empresa1" Type="String" />
                <asp:Parameter Name="documento1" Type="String" />
                <asp:Parameter Name="numero1" Type="Int32" />
                <asp:Parameter Name="valor1" Type="Decimal" />
                <asp:Parameter Name="descricao_desp2" Type="String" />
                <asp:Parameter Name="empresa2" Type="String" />
                <asp:Parameter Name="documento2" Type="String" />
                <asp:Parameter Name="numero2" Type="Int32" />
                <asp:Parameter Name="valor2" Type="Decimal" />
                <asp:Parameter Name="total" Type="Decimal" />
                <asp:Parameter Name="historico" Type="String" />
                <asp:Parameter Name="iss" Type="Double" />
                <asp:Parameter Name="irrf" Type="Double" />
                <asp:Parameter Name="pis" Type="Double" />
                <asp:Parameter Name="cofins" Type="Double" />
                <asp:Parameter Name="csll" Type="Double" />
                <asp:Parameter Name="outros" Type="Double" />
                <asp:Parameter Name="total_deducoes" Type="Double" />
                <asp:Parameter Name="pagamento" Type="String" />
                <asp:Parameter Name="vencimento1" Type="DateTime" />
                <asp:Parameter Name="vencimento2" Type="DateTime" />
                <asp:Parameter Name="vencimento3" Type="DateTime" />
                <asp:Parameter Name="vencimento4" Type="DateTime" />
                <asp:Parameter Name="favorecido" Type="String" />
                <asp:Parameter Name="cnpj_cpf" Type="Int64" />
                <asp:Parameter Name="banco" Type="String" />
                <asp:Parameter Name="agencia" Type="Int32" />
                <asp:Parameter Name="conta" Type="Int32" />
                <asp:Parameter Name="corrente_ou_poup" Type="String" />
                <asp:Parameter Name="valor_pag" Type="Double" />
                <asp:Parameter Name="alteracao" Type="DateTime" />
                <asp:Parameter Name="usuario" Type="String" />
                <asp:Parameter Name="mes_despesa" Type="String" />
                <asp:Parameter Name="ano_despesa" Type="Int32" />
                <asp:Parameter Name="categoria_despesa" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="empresa" Type="String" />
                <asp:Parameter Name="data" Type="DateTime" />
                <asp:Parameter Name="numero" Type="Int32" />
                <asp:Parameter Name="requisitante" Type="String" />
                <asp:Parameter Name="unidade" Type="String" />
                <asp:Parameter Name="departamento" Type="String" />
                <asp:Parameter Name="descricao_desp1" Type="String" />
                <asp:Parameter Name="empresa1" Type="String" />
                <asp:Parameter Name="documento1" Type="String" />
                <asp:Parameter Name="numero1" Type="Int32" />
                <asp:Parameter Name="valor1" Type="Decimal" />
                <asp:Parameter Name="descricao_desp2" Type="String" />
                <asp:Parameter Name="empresa2" Type="String" />
                <asp:Parameter Name="documento2" Type="String" />
                <asp:Parameter Name="numero2" Type="Int32" />
                <asp:Parameter Name="valor2" Type="Decimal" />
                <asp:Parameter Name="total" Type="Decimal" />
                <asp:Parameter Name="historico" Type="String" />
                <asp:Parameter Name="iss" Type="Double" />
                <asp:Parameter Name="irrf" Type="Double" />
                <asp:Parameter Name="pis" Type="Double" />
                <asp:Parameter Name="cofins" Type="Double" />
                <asp:Parameter Name="csll" Type="Double" />
                <asp:Parameter Name="outros" Type="Double" />
                <asp:Parameter Name="total_deducoes" Type="Double" />
                <asp:Parameter Name="pagamento" Type="String" />
                <asp:Parameter Name="vencimento1" Type="DateTime" />
                <asp:Parameter Name="vencimento2" Type="DateTime" />
                <asp:Parameter Name="vencimento3" Type="DateTime" />
                <asp:Parameter Name="vencimento4" Type="DateTime" />
                <asp:Parameter Name="favorecido" Type="String" />
                <asp:Parameter Name="cnpj_cpf" Type="Int64" />
                <asp:Parameter Name="banco" Type="String" />
                <asp:Parameter Name="agencia" Type="Int32" />
                <asp:Parameter Name="conta" Type="Int32" />
                <asp:Parameter Name="corrente_ou_poup" Type="String" />
                <asp:Parameter Name="valor_pag" Type="Double" />
                <asp:Parameter Name="alteracao" Type="DateTime" />
                <asp:Parameter Name="usuario" Type="String" />
                <asp:Parameter Name="mes_despesa" Type="String" />
                <asp:Parameter Name="ano_despesa" Type="Int32" />
                <asp:Parameter Name="categoria_despesa" Type="String" />
                <asp:Parameter Name="Original_ID" Type="Int32" />
            </UpdateParameters>
            </asp:ObjectDataSource>
   
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
   
            <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" DataSourceID="Executar" DataTextField="ID" DataValueField="ID">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load" DataSourceID="Executar" DataTextField="ID" DataValueField="ID">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem></asp:ListItem>
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
   
        <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="811px" Width="799px">
            <LocalReport ReportPath="Relatorio.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="Executar" Name="Requisicao" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>



        </form>
   
</body>
</html>
