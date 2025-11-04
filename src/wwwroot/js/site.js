// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
(function () {
    var checkbox = document.getElementById('checkbox');
    if (checkbox) {
        checkbox.addEventListener('change', function () {
            if (this.checked) {
                document.body.classList.add('dark-mode');
                localStorage.setItem('theme', 'dark-mode');
            } else {
                document.body.classList.remove('dark-mode');
                localStorage.setItem('theme', 'light-mode');
            }
        });

        // Check for saved theme preference
        var theme = localStorage.getItem('theme');
        if (theme === 'dark-mode') {
            document.body.classList.add('dark-mode');
            checkbox.checked = true;
        }
    }
})();
