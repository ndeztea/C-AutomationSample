using NUnit.Framework;
using NUnit;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using System;

[TestFixture]
public class TestProgram
{
    private IWebDriver driver;
   
    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver("/Users/dimas/Works/XMileComputer/libs/driver");
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(6000);
        driver.Url = "http://computer-database.gatling.io/computers";
        
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }

    [Test]
    /**
    * Verify add new computer success
    */
    public void TestAddComputer()
    {
        IWebElement button_AddNewComputer = driver.FindElement(By.Id("add"));
        button_AddNewComputer.Click();
        
        IWebElement input_ComputerName = driver.FindElement(By.Id("name"));
        input_ComputerName.SendKeys("New Data Computer");

        IWebElement button_CreateThisComputer = driver.FindElement(By.CssSelector(".btn.primary"));
        button_CreateThisComputer.Click();

        IWebElement message_AlertWarning = driver.FindElement(By.CssSelector(".alert-message.warning"));

        Assert.AreEqual("Done ! Computer New Data Computer has been created" , message_AlertWarning.Text);
        
    }

    [Test]
    /**
    * Verify add new computer success with injection input
    */ 
    public void TestAddComputerDataInjection()
    {
        IWebElement button_AddNewComputer = driver.FindElement(By.Id("add"));
        button_AddNewComputer.Click();
        
        IWebElement input_ComputerName = driver.FindElement(By.Id("name"));
        input_ComputerName.SendKeys("<table><script>alert('aa')</script>");

        IWebElement button_CreateThisComputer = driver.FindElement(By.CssSelector(".btn.primary"));
        button_CreateThisComputer.Click();

        IWebElement message_AlertWarning = driver.FindElement(By.CssSelector(".alert-message.warning"));

        Assert.AreEqual("Done ! Computer <table><script>alert('aa')</script> has been created" , message_AlertWarning.Text);
        
    }

    /**
    * Verify add new computer failed data empty
    * Verify add new computer failed required date is invalid
    */
    [Test]
    public void TestAddComputerFailed()
    {   
        // check name is empty
        IWebElement button_AddNewComputer = driver.FindElement(By.Id("add"));
        button_AddNewComputer.Click();
        IWebElement button_CreateThisComputer = driver.FindElement(By.CssSelector(".btn.primary"));
        button_CreateThisComputer.Click();
        IWebElement message_AlertWarningName = driver.FindElement(By.XPath("//input[@id='name']/following::span"));
        Assert.AreEqual(message_AlertWarningName.Text, "Failed to refine type : Predicate isEmpty() did not fail.");

        // check format date is not correct
        IWebElement input_ComputerName = driver.FindElement(By.Id("name"));
        input_ComputerName.SendKeys("New Data Computer");
        IWebElement input_Introduced = driver.FindElement(By.Id("introduced"));
        input_Introduced.SendKeys("AAA");
        driver.FindElement(By.CssSelector(".btn.primary")).Click();
        IWebElement message_AlertWarningIntroduced = driver.FindElement(By.XPath("//input[@id='introduced']/following::span"));
        Assert.AreEqual(message_AlertWarningIntroduced.Text, "Failed to decode date : java.time.format.DateTimeParseException: Text 'AAA' could not be parsed at index 0");
    }

    [Test]
    public void TestCancelAddComputer()
    {   
        // check name is empty
        IWebElement button_AddNewComputer = driver.FindElement(By.Id("add"));
        button_AddNewComputer.Click();  
        IWebElement button_Cancel = driver.FindElement(By.XPath("//a[text()='Cancel']"));
        button_Cancel.Click();
        IWebElement h1_Header = driver.FindElement(By.CssSelector("#main h1"));
        Assert.That(h1_Header.Text, Does.Contain("computers found").IgnoreCase);
    }
    
    [Test]
    /**
    * Verify click computer data
    */
    public void TestEditExsistingComputer()
    {   
        // get the first data
        IWebElement a_Data1 = driver.FindElement(By.XPath("//table[@class='computers zebra-striped']/tbody/tr[1]/td/a"));
        a_Data1.Click();
        
        // modify data
        IWebElement input_ComputerName = driver.FindElement(By.Id("name"));
        input_ComputerName.Clear();
        input_ComputerName.SendKeys("Edit Data Computer");

        IWebElement input_Introduced = driver.FindElement(By.Id("introduced"));
        input_Introduced.SendKeys("2010-10-10");
        IWebElement input_Discontinued = driver.FindElement(By.Id("discontinued"));
        input_Discontinued.SendKeys("2021-10-10");

        IWebElement button_CreateThisComputer = driver.FindElement(By.CssSelector(".btn.primary"));
        button_CreateThisComputer.Click();
        // make sure data saved
        IWebElement message_AlertWarning = driver.FindElement(By.CssSelector(".alert-message.warning"));
        Assert.AreEqual("Done ! Computer Edit Data Computer has been updated" , message_AlertWarning.Text);
    }

    [Test]
    /**
    * Verify cancel existing existing computer 
    */
    public void TestcancelEditComputer()
    {   

        IWebElement a_Data1 = driver.FindElement(By.XPath("//table[@class='computers zebra-striped']/tbody/tr[1]/td/a"));
        a_Data1.Click();
        
        IWebElement button_Cancel = driver.FindElement(By.XPath("//a[text()='Cancel']"));
        button_Cancel.Click();
        IWebElement h1_Header = driver.FindElement(By.CssSelector("#main h1"));
        Assert.That(h1_Header.Text, Does.Contain("computers found").IgnoreCase);
    }

    [Test]
    /**
    * Verify edit computer failed required data empty
    * Verify edit computer failed required date is invalid
    */
    public void TestEditExsistingComputerDataFailed()
    {   

        IWebElement a_Data1 = driver.FindElement(By.XPath("//table[@class='computers zebra-striped']/tbody/tr[1]/td/a"));
        a_Data1.Click();
        
        // computer name invalid
        IWebElement input_ComputerName = driver.FindElement(By.Id("name"));
        input_ComputerName.Clear();
        IWebElement button_CreateThisComputer = driver.FindElement(By.CssSelector(".btn.primary"));
        button_CreateThisComputer.Click();
        IWebElement message_AlertWarningName = driver.FindElement(By.XPath("//input[@id='name']/following::span"));
        Assert.AreEqual(message_AlertWarningName.Text, "Failed to refine type : Predicate isEmpty() did not fail.");

        // check format date is not correct
        IWebElement input_ComputerName2 = driver.FindElement(By.Id("name"));
        input_ComputerName2.SendKeys("New Data Computer");
        IWebElement input_Introduced = driver.FindElement(By.Id("introduced"));
        input_Introduced.SendKeys("AAA");
        driver.FindElement(By.CssSelector(".btn.primary")).Click();
        IWebElement message_AlertWarningIntroduced = driver.FindElement(By.XPath("//input[@id='introduced']/following::span"));
        Assert.AreEqual(message_AlertWarningIntroduced.Text, "Failed to decode date : java.time.format.DateTimeParseException: Text 'AAA' could not be parsed at index 0");
    
    }

   [Test]
    /**
    * Verify edit computer failed with injection input
    */
    public void TestEditComputerDataInjection()
    {   

        IWebElement a_Data1 = driver.FindElement(By.XPath("//table[@class='computers zebra-striped']/tbody/tr[1]/td/a"));
        a_Data1.Click();
        
        IWebElement input_ComputerName = driver.FindElement(By.Id("name"));
        input_ComputerName.Clear();
        input_ComputerName.SendKeys("<table><script>alert('aa')</script>");
        IWebElement button_CreateThisComputer = driver.FindElement(By.CssSelector(".btn.primary"));
        button_CreateThisComputer.Click();
        IWebElement message_AlertWarning = driver.FindElement(By.CssSelector(".alert-message.warning"));

        Assert.AreEqual("Done ! Computer <table><script>alert('aa')</script> has been created" , message_AlertWarning.Text);
    }

    [Test]
    /**
    * Verify edit computer and delete data
    */
    public void TestEditAndDeleteComputer()
    {   

        IWebElement a_Data1 = driver.FindElement(By.XPath("//table[@class='computers zebra-striped']/tbody/tr[1]/td/a"));
        a_Data1.Click();
        
        IWebElement button_DeleteThisComputer = driver.FindElement(By.CssSelector(".btn.danger"));
        button_DeleteThisComputer.Click();
        // make sure data deleted
        IWebElement message_AlertWarning = driver.FindElement(By.CssSelector(".alert-message.warning"));
        Assert.AreEqual("Done ! Computer ACE has been deleted" , message_AlertWarning.Text); 
    }

    [Test]
    /**
    * Verify edit computer failed data not exist
    */
    public void TestEditComputerNotExist()
    {   

        driver.Url = "http://computer-database.gatling.io/computers/38100";
        
        // make sure data not exists
        IWebElement h1_Header = driver.FindElement(By.CssSelector("body"));
        Assert.That(h1_Header.Text, Does.Contain("HTTP ERROR 404").IgnoreCase);
    }

    [Test]
    /**
    * Verify table pagination next
    * Verify table pagination next and prev
    */
    public void TestTablePagination()
    {   
 
        // Next pagination test
        IWebElement a_Next = driver.FindElement(By.CssSelector(".next a"));
        a_Next.Click();
        IWebElement a_CurrentPage = driver.FindElement(By.CssSelector("li.current a"));
        Assert.That(a_CurrentPage.Text, Does.Contain("Displaying 11 to 20 ").IgnoreCase);

        // Prev pagination test
        IWebElement a_Prev = driver.FindElement(By.CssSelector(".prev a"));
        a_Prev.Click();
        IWebElement a_CurrentPage2 = driver.FindElement(By.CssSelector("li.current a"));
        Assert.That(a_CurrentPage2.Text, Does.Contain("Displaying 1 to 10 ").IgnoreCase);
    }

    [Test]
    /**
    * Verify search computer success
    * Verify search computer data not exists
    */
    public void TestSearchComputer()
    {   
 
        // Search not exists
        IWebElement input_Search = driver.FindElement(By.Id("searchbox"));
        input_Search.SendKeys("Blah");
        IWebElement btn_FilterByName = driver.FindElement(By.Id("searchsubmit"));
        btn_FilterByName.Click();
        // make sure data not exists
        IWebElement body_Text = driver.FindElement(By.CssSelector("body"));
        Assert.That(body_Text.Text, Does.Contain("Nothing to display").IgnoreCase);

        // Search success
        IWebElement input_Search2 = driver.FindElement(By.Id("searchbox"));
        input_Search2.Clear();
        input_Search2.SendKeys("Softcard");
        IWebElement btn_FilterByName2 = driver.FindElement(By.Id("searchsubmit"));
        btn_FilterByName2.Click();
        IWebElement body_Text2 = driver.FindElement(By.CssSelector("tbody"));
        Assert.That(body_Text2.Text, Does.Contain("Microsoft Softcard").IgnoreCase);
    }
    
}