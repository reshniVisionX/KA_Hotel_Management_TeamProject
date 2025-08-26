function loadComponent(selector, file) {
  fetch(file)
    .then(res => res.text())
    .then(data => {
      document.querySelector(selector).innerHTML = data;
    });
}

window.addEventListener('DOMContentLoaded', () => {
  loadComponent("#navbar", "Navbar.html");
  loadComponent("#footer", "Footer.html");
});
