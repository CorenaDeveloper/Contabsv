function floatingDropdownForTable(tableSelector) {
    let dropdownMenu;

    $(document).on('show.bs.dropdown', `${tableSelector} .dropdown`, function () {
        const $dropdown = $(this);
        const $button = $dropdown.find('.dropdown-toggle');

        dropdownMenu = $dropdown.find('.dropdown-menu');
        $('body').append(dropdownMenu.detach());

        const eOffset = $button.offset();
        const dropdownHeight = $button.outerHeight();

        dropdownMenu.css({
            'display': 'block',
            'position': 'absolute',
            'top': eOffset.top + dropdownHeight,
            'left': eOffset.left,
            'z-index': 9999
        });

        $(window).on('scroll.dropdownMenu resize.dropdownMenu', function () {
            const newOffset = $button.offset();
            dropdownMenu.css({
                'top': newOffset.top + dropdownHeight,
                'left': newOffset.left
            });
        });
    });

    $(document).on('hide.bs.dropdown', `${tableSelector} .dropdown`, function () {
        const $dropdown = $(this);

        $(window).off('scroll.dropdownMenu resize.dropdownMenu');

        $dropdown.append(dropdownMenu.detach());
        dropdownMenu.hide();
    });
}