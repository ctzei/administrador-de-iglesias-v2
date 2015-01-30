/**
 * Generic Helper Functions
 */

function getQueryString() {
    var rtn = {};
    var qs = location.search.substr(1).replace(/\+/g, ' ').split('&');
    for (var i = 0; i < qs.length; i++) {
        qs[i] = qs[i].split('=');
        if (qs[i][0])
            rtn[qs[i][0]] = decodeURIComponent(qs[i][1]);
    }
    return rtn;
}

function ResolveUrl(url) {
    if (url.indexOf("~/") == 0) {
        url = baseUrl + url.substring(2);
    }
    return url;
}

function getFileExtension(filename) {
    var ext = /^.+\.([^.]+)$/.exec(filename);
    return ext == null ? "" : ext[1];
}

function getUTCDateFromMSSQL(date) {
    var y = date.substr(0, 4), //Year
        m = date.substr(5, 2), //Month
        d = date.substr(8, 2); //Day
    return Date.UTC(y, m - 1, d);
}

function getShortDateFromMSSQL(date) {
    var y = date.substr(0, 4), //Year
    m = date.substr(5, 2), //Month
    d = date.substr(8, 2); //Day
    return (new Date(y, m - 1, d)).format("d/M/y");
}

var tableToCSV = function ($table, filename) {
    var $rows = $table.find('tr:has(td)'),

    // Temporary delimiter characters unlikely to be typed by keyboard
    // This is to avoid accidentally splitting the actual contents
    tmpColDelim = String.fromCharCode(11), // vertical tab character
    tmpRowDelim = String.fromCharCode(0), // null character

    // actual delimiter characters for CSV format
    colDelim = '","', rowDelim = '"\r\n"',

    // Grab text from table into CSV formatted string
    csv = '"' + $rows.map(function (i, row) {
        var $row = $(row), $cols = $row.find('th, td');

        return $cols.map(function (j, col) {
            var $col = $(col), text = $col.text();
            return text.replace('"', '""'); // escape double quotes

        }).get().join(tmpColDelim);

    }).get().join(tmpRowDelim).split(tmpRowDelim).join(rowDelim).split(tmpColDelim).join(colDelim) + '"',

    // Data URI
    csvData = 'data:application/csv;charset=utf-8,' + encodeURIComponent(csv);

    $(this).attr({
        'download': filename,
        'href': csvData,
        'target': '_blank'
    });
};