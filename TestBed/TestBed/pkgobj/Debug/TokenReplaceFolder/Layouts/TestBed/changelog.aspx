﻿<%@ Assembly Name="TestBed, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d48b244bc905446f" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page ContentType="text/html; charset=utf-8" Language="C#" AutoEventWireup="true" CodeBehind="changelog.aspx.cs" Inherits="TestBed.Layouts.TestBed.changelog" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/angular.js/1.6.5/angular.min.js"></script>
    <script type="text/javascript" src="scripts/changelog.js"></script>
    <script data-require="ui-bootstrap@*" data-semver="1.3.2" src="https://cdn.rawgit.com/angular-ui/bootstrap/gh-pages/ui-bootstrap-tpls-1.3.2.js"></script>
    <link rel="stylesheet" type="text/css" href="styles/changelog.css">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <script type="text/javascript">

    </script>

    <div ng-app="myApp" ng-controller="changelog">

        <div ng-repeat="log in changelogs">

            <h1 ng-if="log.Count > 1 || log.Count === 0">
                {{log.Name}} - ({{log.Count}} Entries) <a ng-if="log.Count !== 0" href="{{log.URL}}">View All</a>
            </h1>
            <h1 ng-if="log.Count === 1">{{log.Name}} - ({{log.Count}} Entry)</h1>


            <div ng-repeat="item in log.Changelog">
                <div class="gimmalcard">
                    <div class="gimmalcontainer">
                        <h4>Name: {{item.Name}}</h4>
                        <p>Author: {{item.Author}}</p>
                        <p>Date: {{item.DateString}}</p>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    Hey, that's pretty good
</asp:Content>


