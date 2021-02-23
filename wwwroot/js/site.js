// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



var add = document.getElementById("AddRoom");
var addModal = document.getElementById("addModal");

add.addEventListener("click", function(){
    addModal.classList.add("active");
})

function closeModal(){
    addModal.classList.remove("active");
    
}