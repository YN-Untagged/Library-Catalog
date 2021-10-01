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

//Filter by Book Genre
    $("#genre").click(function () {
        var value = $(this).val().toLowerCase();
        $("#bookCard.card").filter(function () {
            $(this).toggle($(this).find('i').text().toLowerCase().indexOf(value) > -1)
        });
    });

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

