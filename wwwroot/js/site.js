const checkbox = document.getElementById("updateTable");
const idField = document.getElementById("idField");

checkbox.addEventListener("change", function () {
    if (this.checked) {
        idField.style.display = "block"; // visa rutan
    } else {
        idField.style.display = "none"; // dölj rutan
    }
});