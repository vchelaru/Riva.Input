function getVerticalScrollPosition() {
    return document.body.scrollTop.toString();
}

function setVerticalScrollPosition(position) {
    document.body.scrollTop = position;
}

function scrollToBottom() {
    document.body.scrollTop = 10000000;
}

function scrollAtBottom() {
    var totalHeight, currentScroll, visibleHeight;

    if (document.documentElement.scrollTop)
        currentScroll = document.documentElement.scrollTop;
    else
        currentScroll = document.body.scrollTop;

    totalHeight = document.body.offsetHeight;
    visibleHeight = document.documentElement.clientHeight;

    if (totalHeight <= currentScroll + visibleHeight)
        return true;
    else
        return false;
}

function getHorizontalScrollPosition() {
    return document.body.scrollLeft.toString();
}

function setHorizontalScrollPosition(position) {
    document.body.scrollLeft = position;
}