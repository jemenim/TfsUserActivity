using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using TfsUserActivity.Core.Messaging;

namespace TfsUserActivity.Core
{
    public class OneBoardExporter : IDisposable
    {
        private const string LoginUrl = "http://agilesvv.dyndns.org/OneBoard/Login.aspx";
        private const string TimesheetUrl = "http://agilesvv.dyndns.org/OneBoard/Timesheet.aspx";

        private readonly IWebDriver _drv = new ChromeDriver();

        public void ExportSheets(string login, string password, OneBoardSheet[] sheets)
        {
            var wait = new WebDriverWait(_drv, new TimeSpan(0, 0, 5));

            _drv.Navigate().GoToUrl(LoginUrl);
            _drv.FindElement(By.XPath("//input[@name='txtLogin']")).SendKeys(login);
            _drv.FindElement(By.XPath("//input[@name='txtPassword']")).SendKeys(password);
            _drv.FindElement(By.XPath("//input[@name='btnLogin']")).Click();
            foreach (var sheet in sheets)
            {
                _drv.Navigate().GoToUrl(TimesheetUrl);
                new SelectElement(_drv.FindElement(By.XPath("//select[contains(@name,'ddlProject')]"))).SelectByText(sheet.Project);
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//option[text()='" + sheet.Task + "']")));
                new SelectElement(_drv.FindElement(By.XPath("//select[contains(@name,'ddlTask')]"))).SelectByText(sheet.Task);

                var date = _drv.FindElement(By.XPath("//input[contains(@name,'txtDate')]"));
                date.Clear();
                date.SendKeys(sheet.Date.ToString(@"MM\/dd\/yyyy"));

                var duration = _drv.FindElement(By.XPath("//input[contains(@name,'txtDuration')]"));
                _drv.ExecuteJavaScript<bool>("arguments[0].value = arguments[1]; return true;", duration, sheet.Duration.ToString(@"hh\:mm"));

                _drv.FindElement(By.XPath("//input[contains(@name,'txtUrl')]")).SendKeys(sheet.Url);
                _drv.FindElement(By.XPath("//textarea[contains(@name,'txtComment')]")).SendKeys(sheet.Comment);

                _drv.FindElement(By.XPath("//input[contains(@name,'btnSave')]")).Click();
            }
        }

        public void Dispose()
        {
            _drv.Quit();
        }
    }
}
