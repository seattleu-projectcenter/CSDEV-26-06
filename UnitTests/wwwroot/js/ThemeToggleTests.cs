using NUnit.Framework;

namespace UnitTests.ThemeToggle
{
    /// <summary>
    /// Unit tests for the light/dark mode toggle functionality
    /// Tests the JavaScript-based theme switching feature located in site.js
    ///
    /// The theme toggle is implemented in:
    /// - site.js: JavaScript logic for theme switching and localStorage persistence
    /// - site.css: CSS styles for light/dark mode and toggle button
    /// - _Layout.cshtml: HTML structure for the toggle button (lines 64-69)
    ///
    /// Functionality tested:
    /// - Theme toggle button presence and structure
    /// - Dark mode class application to body
    /// - LocalStorage persistence of theme preference
    /// - Theme restoration on page load
    /// </summary>
    [TestFixture]
    public class ThemeToggleTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region ThemeToggleStructure

        /// <summary>
        /// Test to verify that the theme toggle HTML structure exists in _Layout.cshtml
        /// The toggle should be present in the footer section
        /// Located at lines 64-69 in _Layout.cshtml
        /// </summary>
        [Test]
        public void ThemeToggle_Structure_Exists_In_Layout()
        {
            // Arrange
            var layoutPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\Pages\Shared\_Layout.cshtml";

            // Act
            var layoutContent = System.IO.File.ReadAllText(layoutPath);

            // Assert
            Assert.IsTrue(layoutContent.Contains("theme-switch-wrapper"), "Layout should contain theme-switch-wrapper div");
            Assert.IsTrue(layoutContent.Contains("id=\"checkbox\""), "Layout should contain checkbox input with id 'checkbox'");
            Assert.IsTrue(layoutContent.Contains("class=\"slider round\""), "Layout should contain slider with 'slider round' class");
        }

        #endregion ThemeToggleStructure

        #region ThemeToggleJavaScript

        /// <summary>
        /// Test to verify that the theme toggle JavaScript logic exists in site.js
        /// The script should handle checkbox changes and localStorage operations
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Exists_In_SiteJs()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("getElementById('checkbox')"), "site.js should get checkbox element");
            Assert.IsTrue(siteJsContent.Contains("dark-mode"), "site.js should reference 'dark-mode' class");
            Assert.IsTrue(siteJsContent.Contains("localStorage.setItem"), "site.js should save theme to localStorage");
            Assert.IsTrue(siteJsContent.Contains("localStorage.getItem('theme')"), "site.js should retrieve theme from localStorage");
        }

        /// <summary>
        /// Test to verify that the JavaScript adds 'dark-mode' class when checkbox is checked
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Adds_DarkMode_Class()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("classList.add('dark-mode')"),
                "site.js should add 'dark-mode' class to body when checked");
        }

        /// <summary>
        /// Test to verify that the JavaScript removes 'dark-mode' class when checkbox is unchecked
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Removes_DarkMode_Class()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("classList.remove('dark-mode')"),
                "site.js should remove 'dark-mode' class from body when unchecked");
        }

        /// <summary>
        /// Test to verify that the JavaScript persists theme preference to localStorage when checkbox is checked
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Saves_DarkMode_To_LocalStorage()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("localStorage.setItem('theme', 'dark-mode')"),
                "site.js should save 'dark-mode' to localStorage when checkbox is checked");
        }

        /// <summary>
        /// Test to verify that the JavaScript persists theme preference to localStorage when checkbox is unchecked
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Saves_LightMode_To_LocalStorage()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("localStorage.setItem('theme', 'light-mode')"),
                "site.js should save 'light-mode' to localStorage when checkbox is unchecked");
        }

        /// <summary>
        /// Test to verify that the JavaScript restores dark mode from localStorage on page load
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Restores_DarkMode_From_LocalStorage()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("var theme = localStorage.getItem('theme')"),
                "site.js should retrieve theme from localStorage");
            Assert.IsTrue(siteJsContent.Contains("theme === 'dark-mode'"),
                "site.js should check if saved theme is 'dark-mode'");
            Assert.IsTrue(siteJsContent.Contains("checkbox.checked = true"),
                "site.js should set checkbox to checked if dark mode was saved");
        }

        /// <summary>
        /// Test to verify that the JavaScript uses an IIFE (Immediately Invoked Function Expression)
        /// This ensures the code runs immediately and doesn't pollute the global scope
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Uses_IIFE_Pattern()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("(function ()") || siteJsContent.Contains("(function()"),
                "site.js should use IIFE pattern to encapsulate theme toggle code");
            Assert.IsTrue(siteJsContent.Contains("})();"),
                "site.js should properly close IIFE with })();");
        }

        /// <summary>
        /// Test to verify that the JavaScript handles null checkbox gracefully
        /// Prevents errors if the checkbox element doesn't exist on the page
        /// </summary>
        [Test]
        public void ThemeToggle_JavaScript_Handles_Null_Checkbox()
        {
            // Arrange
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";

            // Act
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);

            // Assert
            Assert.IsTrue(siteJsContent.Contains("if (checkbox)"),
                "site.js should check if checkbox exists before adding event listener");
        }

        #endregion ThemeToggleJavaScript

        #region ThemeToggleCSS

        /// <summary>
        /// Test to verify that dark mode CSS styles exist in site.css
        /// The body.dark-mode class should change the background and text color
        /// </summary>
        [Test]
        public void ThemeToggle_CSS_DarkMode_Styles_Exist()
        {
            // Arrange
            var siteCssPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\css\site.css";

            // Act
            var siteCssContent = System.IO.File.ReadAllText(siteCssPath);

            // Assert
            Assert.IsTrue(siteCssContent.Contains("body.dark-mode"),
                "site.css should contain body.dark-mode selector");
        }

        /// <summary>
        /// Test to verify that the theme toggle button styles exist in site.css
        /// </summary>
        [Test]
        public void ThemeToggle_CSS_Toggle_Button_Styles_Exist()
        {
            // Arrange
            var siteCssPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\css\site.css";

            // Act
            var siteCssContent = System.IO.File.ReadAllText(siteCssPath);

            // Assert
            Assert.IsTrue(siteCssContent.Contains(".theme-switch-wrapper"),
                "site.css should contain .theme-switch-wrapper styles");
            Assert.IsTrue(siteCssContent.Contains(".theme-switch"),
                "site.css should contain .theme-switch styles");
            Assert.IsTrue(siteCssContent.Contains(".slider"),
                "site.css should contain .slider styles");
        }

        /// <summary>
        /// Test to verify that the toggle slider has rounded corners style
        /// </summary>
        [Test]
        public void ThemeToggle_CSS_Slider_Has_Round_Style()
        {
            // Arrange
            var siteCssPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\css\site.css";

            // Act
            var siteCssContent = System.IO.File.ReadAllText(siteCssPath);

            // Assert
            Assert.IsTrue(siteCssContent.Contains(".slider.round"),
                "site.css should contain .slider.round styles for rounded toggle");
        }

        /// <summary>
        /// Test to verify that the toggle has a checked state style
        /// The slider should change position when the input is checked
        /// </summary>
        [Test]
        public void ThemeToggle_CSS_Has_Checked_State_Style()
        {
            // Arrange
            var siteCssPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\css\site.css";

            // Act
            var siteCssContent = System.IO.File.ReadAllText(siteCssPath);

            // Assert
            Assert.IsTrue(siteCssContent.Contains("input:checked + .slider"),
                "site.css should contain styles for checked state");
        }

        /// <summary>
        /// Test to verify that the toggle button is positioned in the bottom right corner
        /// </summary>
        [Test]
        public void ThemeToggle_CSS_Positioned_Bottom_Right()
        {
            // Arrange
            var siteCssPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\css\site.css";

            // Act
            var siteCssContent = System.IO.File.ReadAllText(siteCssPath);

            // Assert
            // The theme-switch-wrapper should be positioned absolutely in bottom right
            var wrapperSection = siteCssContent.Substring(
                siteCssContent.IndexOf(".theme-switch-wrapper"),
                siteCssContent.IndexOf("}", siteCssContent.IndexOf(".theme-switch-wrapper")) -
                siteCssContent.IndexOf(".theme-switch-wrapper")
            );

            Assert.IsTrue(wrapperSection.Contains("position: absolute") || wrapperSection.Contains("position:absolute"),
                "theme-switch-wrapper should be absolutely positioned");
            Assert.IsTrue(wrapperSection.Contains("right:") || wrapperSection.Contains("right :"),
                "theme-switch-wrapper should be positioned from the right");
            Assert.IsTrue(wrapperSection.Contains("bottom:") || wrapperSection.Contains("bottom :"),
                "theme-switch-wrapper should be positioned from the bottom");
        }

        #endregion ThemeToggleCSS

        #region IntegrationTests

        /// <summary>
        /// Integration test to verify all three components work together
        /// - HTML structure in _Layout.cshtml
        /// - JavaScript logic in site.js
        /// - CSS styles in site.css
        /// </summary>
        [Test]
        public void ThemeToggle_Integration_All_Components_Present()
        {
            // Arrange
            var layoutPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\Pages\Shared\_Layout.cshtml";
            var siteJsPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\js\site.js";
            var siteCssPath = @"C:\Users\Daniel Kirov-Tomilov\Desktop\2025_26_SU\clones\CSDEV-26-06\src\wwwroot\css\site.css";

            // Act
            var layoutContent = System.IO.File.ReadAllText(layoutPath);
            var siteJsContent = System.IO.File.ReadAllText(siteJsPath);
            var siteCssContent = System.IO.File.ReadAllText(siteCssPath);

            // Assert - Verify all three components reference 'dark-mode'
            Assert.IsTrue(siteJsContent.Contains("dark-mode"), "JavaScript should handle dark-mode class");
            Assert.IsTrue(siteCssContent.Contains("dark-mode"), "CSS should style dark-mode");

            // Verify all components reference the checkbox
            Assert.IsTrue(layoutContent.Contains("id=\"checkbox\""), "Layout should have checkbox element");
            Assert.IsTrue(siteJsContent.Contains("getElementById('checkbox')"), "JavaScript should target checkbox");

            // Verify all components reference the slider
            Assert.IsTrue(layoutContent.Contains("slider round"), "Layout should have slider element");
            Assert.IsTrue(siteCssContent.Contains(".slider"), "CSS should style slider");
        }

        #endregion IntegrationTests
    }
}
