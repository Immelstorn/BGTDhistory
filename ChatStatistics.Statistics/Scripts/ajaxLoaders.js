var startLoading = function() {
    $.ajax({
        url: '/Home/GetGeneralStats',
        beforeSend: function() { $('#general').html('Loading...'); }
    }).done(function(result) {
        generalChart(result);
    });

    $.ajax({
        url: '/Home/GetGeneralStatsByUser',
        beforeSend: function() { $('#generalByUser').html('Loading...'); }
    }).done(function(result) {
        generalByUserChart(result);
    });

    $.ajax({
        url: '/Home/GetUsersStats',
        beforeSend: function() { $('#users').html('Loading...'); }
    }).done(function(result) {
        usersChart(result);
    });

    $.ajax({
        url: '/Home/GetUsersBySymbolsCountStats',
        beforeSend: function() { $('#usersBySymbols').html('Loading...'); }
    }).done(function(result) {
        usersBySymbolsChart(result);
    });

    $.ajax({
        url: '/Home/GetUsersMessageSizeStats',
        beforeSend: function() { $('#usersBySize').html('Loading...'); }
    }).done(function(result) {
        usersBySizeChart(result);
    });

    $.ajax({
        url: '/Home/GetOnlyWithAttachments',
        beforeSend: function() { $('#usersWithAttachments').html('Loading...'); }
    }).done(function(result) {
        usersWithAttachmentsChart(result);
    });

    $.ajax({
        url: '/Home/GetWordsStatistics',
        beforeSend: function() { $('#words').html('Loading...'); }
    }).done(function(result) {
        wordsChart(result);
    });

    $.ajax({
        method: "POST",
        url: '/Home/GetOneWordStatistics',
        data: { word: "хуй" },
        beforeSend: function() { $('#wordsByUser').html('Loading...'); }
    }).done(function(result) {
        wordsByUserChart(result);
    });

    $.ajax({
        method: "POST",
        url: '/Home/GetOneWordStatisticsTimelineByUser',
        data: { word: "хуй" },
        beforeSend: function() { $('#wordsByUserTimeline').html('Loading...'); }
    }).done(function(result) {
        wordsByUserTimelineChart(result);
    });

    $('#submitWordByUser').click(function() {
        $.ajax({
            method: "POST",
            url: '/Home/GetOneWordStatistics',
            data: { word: $('#wordByUser').val() },
            beforeSend: function() { $('#wordsByUser').html('Loading...'); }
        }).done(function(result) {
            wordsByUserChart(result);
        });
    });

    $('#submitWordByUserTimeline').click(function() {
        $.ajax({
            method: "POST",
            url: '/Home/GetOneWordStatisticsTimelineByUser',
            data: { word: $('#wordByUserTimeline').val() },
            beforeSend: function() { $('#wordsByUserTimeline').html('Loading...'); }
        }).done(function(result) {
            wordsByUserTimelineChart(result);
        });
    });
}