  $(function () {
            $('#connectToServer').click(function () {
                var host = $('#host');
                if (!validate(host)) {
                    return;
                };
                var port = $('#port');
                if (!validate(port)) {
                    return;
                };
                var url = `http://${host.val()}:${port.val()}/signalr`;

                $.getScript(url + '/hubs').done(function () {
                    $.connection.hub.url = url;
                    var chat = $.connection.desktopHub;
                    chat.client.sendMessage = function (message) {
                        var div = walkJson(JSON.parse(message));
                        $('#discussion').append(div).html();
                    };
                    $('#message').focus();

                    $.connection.hub.start({ transport: ['webSockets'] }).done(function () {
                        $('#connectToServer').prop('disabled', true);
                        $('#sendmessage').click(function () {
                            chat.server.send($('#message').val());
                            $('#message').val('').focus();
                        });
                    });
                })
            });

            function validate(input) {
                if (input.val() === undefined || input.val().length === 0
                    || (input.selector === '#port'
                        && (input.val() < 0 || input.val() > 65535 || isNaN(input.val())))) {
                    input.next().removeClass('error');
                    input.next().addClass('error_show');
                    return false;
                }
                input.next().removeClass('error_show');
                input.next().addClass('error');
                return true
            }

            function walkJson(obj) {
                var result = $('<div />');
                for (const prop in obj) {
                    const value = obj[prop];
                    var spanKey = $('<span/>').text(prop).css("font-weight", "Bold");
                    var spanColon = $('<span/>').text(":").css("font-weight", "Bold");
                    result.append(spanKey).append(spanColon);
                    if (typeof value === 'object') {
                        var div = walkJson(value).css("margin-left", "15px")
                        result.append(div);
                    }
                    else {
                        var spanValue = $('<span/>').text(" " + value)
                        div = $('<div />').append(spanValue);
                        result.append(div);
                    }
                }
                return result;
            }
        });