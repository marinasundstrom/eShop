window.blazorCulture = {
  get: () => window.localStorage['BlazorCulture'],
  set: (value) => window.localStorage['BlazorCulture'] = value
};

window.isDarkMode = () => {
  if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return true;
  }
  return false;
};

window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', async event => {
  await DotNet.invokeMethodAsync("Portal.Shell", "OnDarkModeChanged", event.matches);
});