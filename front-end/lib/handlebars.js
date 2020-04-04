const timeago = require('timeago.js');

const helpers = {};

helpers.timeago = (savedTimestamp) => {
    return timeagoInstance.format(savedTimestamp);
};

module.exports = helpers;