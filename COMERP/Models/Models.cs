using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.Design;
using System.Data;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Xml.Linq;
using System;

namespace COMERP.Models
{

    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
    }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? HireDate { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? HeadEmployeeId { get; set; }
        public Employee HeadEmployee { get; set; }
    }

    public class Service
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? DurationHours { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? EventDate { get; set; }
        public string Location { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class ContactFormSubmission
    {
        public int SubmissionId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class CompanyDetails
    {
        public int CompanyDetailsId { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Mission { get; set; }
        public string Vision { get; set; }
        public string CoreValues { get; set; }
    }

    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadEmployeeId { get; set; }
        public Employee LeadEmployee { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class TeamMember
    {
        public int TeamMemberId { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Role { get; set; }
        public DateTime? JoinedDate { get; set; }
    }

    public class SiteSettings
    {
        public int SiteSettingsId { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string LogoUrl { get; set; }
        public string FaviconUrl { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class Slider
    {
        public int SliderId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ImageUrl { get; set; }
        public string LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class Menu
    {
        public int MenuId { get; set; }
        public int? ParentMenuId { get; set; }
        public Menu ParentMenu { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class PageContent
    {
        public int PageContentId { get; set; }
        public string PageName { get; set; }
        public string SectionTitle { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class FooterLink
    {
        public int FooterLinkId { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }

    public class SocialMediaLink
    {
        public int SocialMediaLinkId { get; set; }
        public string Platform { get; set; }
        public string LinkUrl { get; set; }
        public string IconUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }



    #region Tables
    //    CREATE TABLE Company(
    //    CompanyId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(100) NOT NULL,
    //    Description NVARCHAR(MAX),
    //    EstablishedDate DATE,
    //    ContactEmail NVARCHAR(100),
    //    Phone NVARCHAR(20),
    //    Address NVARCHAR(200),
    //    Website NVARCHAR(100)
    //);

    //CREATE TABLE Employee(
    //    EmployeeId INT PRIMARY KEY IDENTITY,
    //    FirstName NVARCHAR(50) NOT NULL,
    //    LastName NVARCHAR(50) NOT NULL,
    //    Position NVARCHAR(50),
    //    Department NVARCHAR(50),
    //    Email NVARCHAR(100),
    //    Phone NVARCHAR(20),
    //    HireDate DATE,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE Department(
    //    DepartmentId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(50) NOT NULL,
    //    Description NVARCHAR(MAX),
    //    HeadEmployeeId INT FOREIGN KEY REFERENCES Employee(EmployeeId)
    //);

    //CREATE TABLE Service(
    //    ServiceId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(100) NOT NULL,
    //    Description NVARCHAR(MAX),
    //    Price DECIMAL(18, 2),
    //    DurationHours INT,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE Project(
    //    ProjectId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(100) NOT NULL,
    //    Description NVARCHAR(MAX),
    //    StartDate DATE,
    //    EndDate DATE,
    //    Status NVARCHAR(20),
    //    ClientId INT FOREIGN KEY REFERENCES Client(ClientId),
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE Client(
    //    ClientId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(100) NOT NULL,
    //    ContactPerson NVARCHAR(100),
    //    Email NVARCHAR(100),
    //    Phone NVARCHAR(20),
    //    Address NVARCHAR(200),
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE News(
    //    NewsId INT PRIMARY KEY IDENTITY,
    //    Title NVARCHAR(100) NOT NULL,
    //    Content NVARCHAR(MAX),
    //    PublishedDate DATE,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE Event(
    //    EventId INT PRIMARY KEY IDENTITY,
    //    Title NVARCHAR(100) NOT NULL,
    //    Description NVARCHAR(MAX),
    //    EventDate DATE,
    //    Location NVARCHAR(200),
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE ContactFormSubmission(
    //    SubmissionId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(100) NOT NULL,
    //    Email NVARCHAR(100) NOT NULL,
    //    Message NVARCHAR(MAX),
    //    SubmissionDate DATETIME DEFAULT GETDATE(),
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE CompanyDetails(
    //    CompanyDetailsId INT PRIMARY KEY IDENTITY,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId),
    //    Mission NVARCHAR(MAX),
    //    Vision NVARCHAR(MAX),
    //    CoreValues NVARCHAR(MAX)
    //);

    //    CREATE TABLE Team(
    //        TeamId INT PRIMARY KEY IDENTITY,
    //    Name NVARCHAR(100) NOT NULL,
    //        Description NVARCHAR(MAX),
    //    LeadEmployeeId INT FOREIGN KEY REFERENCES Employee(EmployeeId),
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE TeamMember(
    //    TeamMemberId INT PRIMARY KEY IDENTITY,
    //    TeamId INT FOREIGN KEY REFERENCES Team(TeamId),
    //    EmployeeId INT FOREIGN KEY REFERENCES Employee(EmployeeId),
    //    Role NVARCHAR(50),
    //    JoinedDate DATE
    //);

    //CREATE TABLE SiteSettings(
    //    SiteSettingsId INT PRIMARY KEY IDENTITY,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId),
    //    LogoUrl NVARCHAR(200),
    //    FaviconUrl NVARCHAR(200),
    //    ContactEmail NVARCHAR(100),
    //    Phone NVARCHAR(20),
    //    Address NVARCHAR(200),
    //    CreatedDate DATETIME DEFAULT GETDATE()
    //);

    //CREATE TABLE Slider(
    //    SliderId INT PRIMARY KEY IDENTITY,
    //    Title NVARCHAR(100),
    //    Subtitle NVARCHAR(200),
    //    ImageUrl NVARCHAR(200) NOT NULL,
    //    LinkUrl NVARCHAR(200),
    //    DisplayOrder INT,
    //    IsActive BIT DEFAULT 1,
    //    CreatedDate DATETIME DEFAULT GETDATE(),
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE Menu(
    //    MenuId INT PRIMARY KEY IDENTITY,
    //    ParentMenuId INT NULL REFERENCES Menu(MenuId), -- Self-referencing for submenus
    //    Title NVARCHAR(100) NOT NULL,
    //    LinkUrl NVARCHAR(200),
    //    DisplayOrder INT,
    //    IsVisible BIT DEFAULT 1,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE PageContent(
    //    PageContentId INT PRIMARY KEY IDENTITY,
    //    PageName NVARCHAR(100) NOT NULL,
    //    SectionTitle NVARCHAR(100),
    //    Content NVARCHAR(MAX),
    //    ImageUrl NVARCHAR(200),
    //    DisplayOrder INT,
    //    IsVisible BIT DEFAULT 1,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE FooterLink(
    //    FooterLinkId INT PRIMARY KEY IDENTITY,
    //    Title NVARCHAR(100) NOT NULL,
    //    LinkUrl NVARCHAR(200),
    //    DisplayOrder INT,
    //    IsVisible BIT DEFAULT 1,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);

    //CREATE TABLE SocialMediaLink(
    //    SocialMediaLinkId INT PRIMARY KEY IDENTITY,
    //    Platform NVARCHAR(50) NOT NULL, -- e.g., Facebook, Twitter, LinkedIn
    //    LinkUrl NVARCHAR(200),
    //    IconUrl NVARCHAR(200), -- URL for the icon image
    //    DisplayOrder INT,
    //    IsVisible BIT DEFAULT 1,
    //    CompanyId INT FOREIGN KEY REFERENCES Company(CompanyId)
    //);


    #endregion
}
