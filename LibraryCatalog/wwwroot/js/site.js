/** Random Colors for book shelf **/
$(document).ready(function () {
    $('.shelfbook').each(function () {
        var hue = 'rgb(' + (Math.floor((256 - 199) * Math.random()) + 200) + ',' + (Math.floor((256 - 199) * Math.random()) + 200) + ',' + (Math.floor((256 - 199) * Math.random()) + 200) + ')';
        $(this).css("background-color", hue);
    });
});

/**Sticky Navigator **/
window.onscroll = function () { myFunction() };

var navbar = document.getElementById("navbar");
var sticky = navbar.offsetTop;

function myFunction() {
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
    } else {
        navbar.classList.remove("sticky");
    }
}

/*Search Functionality*/

//Search by Book Title
$(document).ready(function () {
    $("#SearchTerm").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#bookCard.card").filter(function () {
            $(this).toggle($(this).find('b').text().toLowerCase().indexOf(value) > -1)
        });
    });
});

function SearchTable() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

//Selectlist with check boxes
$(document).ready(function () {
    $("#authors").CreateMultiCheckBox({
        width: '230px',
        defaultText: 'Select Below', height: '250px'
    });
});

$(function () {
    $('select[multiple].active.authors').multiselect({
        columns: 3,
        placeholder: 'Select Authors',
        search: true,
        searchOptions: {
            'default': 'Search Authors'
        },
        selectAll: false
    });
})

/*Side Panel */
function openNav() {
    document.getElementById("mySidepanel").style.width = "250px";
}

function closeNav() {
    document.getElementById("mySidepanel").style.width = "0";
}

/*DropDown for side panel*/
var dropdown = document.getElementsByClassName("dropdown-btn");
var i;

for (i = 0; i < dropdown.length; i++) {
    dropdown[i].addEventListener("click", function () {
        this.classList.toggle("active");
        var dropdownContent = this.nextElementSibling;
        if (dropdownContent.style.display === "block") {
            dropdownContent.style.display = "none";
        } else {
            dropdownContent.style.display = "block";
        }
    });
}

/*Notification PopUp*/
$(document).ready(function () {
    $('.toast__close').click(function (e) {
        e.preventDefault();
        var parent = $(this).parent('.toast');
        parent.fadeOut("slow", function () { $(this).remove(); });
    });
});

/*Slder*/
$(document).ready(function () {
    $('#autoWidth').lightSlider({
        autoWidth: true,
        loop: true,
        onSliderLoad: function () {
            $('#autoWidth').removeClass('cS-hidden');
        }
    });
});

/*Show more- show less*/
$(document).ready(function () {
    $(".toggle_btn").click(function () {
        $(this).toggleClass("active");
        $(".wrapper ul").toggleClass("active");

        if ($(".toggle_btn").hasClass("active")) {
            $(".toggle_text").text("Show Less");
        }
        else {
            $(".toggle_text").text("Show More");
        }
    });
});

