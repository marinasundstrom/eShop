function splashscreen() {
    let preferredColorScheme = JSON.parse(window.localStorage["preferredColorScheme"] ?? "null");
    let colorScheme = preferredColorScheme ?? (window.isDarkMode() ? 1 : 0);

    if (colorScheme == 1) {
        const elem = document.getElementById("splashscreen");
        elem.classList.toggle("dark");
    }
}

splashscreen();