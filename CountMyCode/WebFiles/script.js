
function animateNumber(targetNumber, element, baseDuration = 1000, padLength = 3, decimalPlaces = 0) {
    const variation = baseDuration * 0.1; // ±10% variation
    const duration = baseDuration + (Math.random() * variation * 2 - variation);

    let startTimestamp = null;
    const startNumber = 0;

    const step = (timestamp) => {
        if (!startTimestamp) startTimestamp = timestamp;
        const progress = Math.min((timestamp - startTimestamp) / duration, 1);
        const currentValue = progress * (targetNumber - startNumber) + startNumber;

        // Round and pad
        const integerPart = Math.floor(currentValue).toString().padStart(padLength, '0');
        const decimalPart = decimalPlaces > 0
            ? '.' + currentValue.toFixed(decimalPlaces).split('.')[1]
            : '';

        element.textContent = integerPart + decimalPart;

        if (progress < 1) {
            requestAnimationFrame(step);
        }
    };

    requestAnimationFrame(step);
}

async function init() {

    // Fetch Data

    var data = null;

    try {
        const res = await fetch('/api/auditdata');
        if (res.ok) {
            data = await res.json();
        } else {
            throw new Error("Could not fetch audit.")
        }
    } catch (error) {
        console.error(error);
    }

    // Counting

    const files = document.getElementById('files');
    animateNumber(data?.files ?? 0, files, 1500);

    const linesOfCode = document.getElementById('lines-code');
    animateNumber(data?.linesOfCode ?? 0, linesOfCode, 1500);

    const characters = document.getElementById('characters');
    animateNumber(data?.characters ?? 0, characters, 1500);

    const languages = document.getElementById('languages');
    animateNumber(data?.languages ?? 0, languages, 1500);

    const kbOfCode = document.getElementById('kb-code');
    animateNumber(data?.kbOfCode ?? 0, kbOfCode, 1500);

    const todos = document.getElementById('todos');
    animateNumber(data?.todos ?? 0, todos, 1500);

    const avgLinesPerFile = document.getElementById('avg-lines-file');
    animateNumber(data?.avgLinesPerFile ?? 0, avgLinesPerFile, 1500);

    const avgCharsPerFile = document.getElementById('avg-chars-file');
    animateNumber(data?.avgCharsPerFile ?? 0, avgCharsPerFile, 1500);

    const avgKbPerFile = document.getElementById('avg-kb-file');
    animateNumber(data?.avgKbPerFile ?? 0, avgKbPerFile, 1500);

    // Pie Charts

    const ctx = document.getElementById('files-by-language').getContext('2d');

    new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Apples', 'Bananas', 'Cherries'],
            datasets: [{
                data: [10, 20, 30],
                backgroundColor: ['red', 'yellow', 'pink']
            }]
        },
        options: {
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });

    // VS

    const emptyLinesVs = document.getElementById('empty-lines-vs');
    animateNumber(data?.emptyLinesVs ?? 0, emptyLinesVs, 2000, 0);

    const linesOfCodeVs = document.getElementById('lines-code-vs');
    animateNumber(100 - (data?.emptyLinesVs ?? 0), linesOfCodeVs, 2000, 0);

    const whitespaceVs = document.getElementById('whitespace-vs');
    animateNumber(data?.whiteSpaceVs ?? 0, whitespaceVs, 2000, 0);

    const codeCharactersVs = document.getElementById('code-chars-vs');
    animateNumber(100 - (data?.whiteSpaceVs ?? 0), codeCharactersVs, 2000, 0);

    // Records

    const largestByKb = document.getElementById('largest-by-kb');
    animateNumber(data?.largestByKb ?? 0, largestByKb, 2000, 0, 1);
    document.getElementById('largest-by-kb-file').textContent = data?.largestByKbFile ?? "~";

    const largestByChars = document.getElementById('largest-by-chars');
    animateNumber(data?.largestByChars ?? 0, largestByChars, 2000, 0);
    document.getElementById('largest-by-chars-file').textContent = data?.largestByCharsFile ?? "~";

    const largestByLines = document.getElementById('largest-by-lines');
    animateNumber(data?.largestByLines ?? 0, largestByLines, 2000, 0);
    document.getElementById('largest-by-lines-file').textContent = data?.largestByLinesFile ?? "~";

    const highestDensity = document.getElementById('highest-density');
    animateNumber(data?.highestDensity ?? 0, highestDensity, 2000, 0, 1);
    document.getElementById('highest-density-file').textContent = data?.highestDensityFile ?? "~";

    const oldestFileDays = document.getElementById('oldest-file-days');
    animateNumber(data?.oldestFileDays ?? 0, oldestFileDays, 2000, 0);
    document.getElementById('oldest-file').textContent = data?.oldestFile ?? "~";

    const newestFileDays = document.getElementById('newest-file-days');
    animateNumber(data?.newestFileDays ?? 0, newestFileDays, 2000, 0);
    document.getElementById('newest-file').textContent = data?.newestFile ?? "~";
}

init();

// Event Listeners

document.getElementById('logo').addEventListener('click', () => {
    confetti({
        particleCount: 125,
        spread: 70,
        origin: { y: 0.65 },
    });
});