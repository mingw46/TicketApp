
window.onload = renderPage;

function renderPage() {
    if ($('#btnTable').click(() => {
        console.log('klikniecie tabeli');
    }));
     if ($('#btnClassic').click(() => {
        console.log('klikniecie widokusycznego');
    }));
}