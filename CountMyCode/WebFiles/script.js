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


const params = new URLSearchParams(window.location.search);
const jsonData = params.get("data");
const data = JSON.parse(jsonData);

// Counting

const files = document.getElementById('files');
animateNumber(data?.files ?? 0, files, 1500);

const linesOfCode = document.getElementById('lines-code');
animateNumber(data?.linesOfCode ?? 0, linesOfCode, 1500);

const characters = document.getElementById('characters');
animateNumber(data?.characters ?? 0, characters, 1500);

const languages = document.getElementById('languages');
animateNumber(data?.languages ?? 0, languages, 1500);

const mbOfCode = document.getElementById('mb-code');
animateNumber(data?.mbOfCode ?? 0, mbOfCode, 1500);

const todos = document.getElementById('todos');
animateNumber(data?.todos ?? 0, todos, 1500);

const avgLinesPerFile = document.getElementById('avg-lines-file');
animateNumber(data?.avgLinesPerFile ?? 0, avgLinesPerFile, 1500);

const avgCharsPerFile = document.getElementById('avg-chars-file');
animateNumber(data?.avgCharsPerFile ?? 0, avgCharsPerFile, 1500);

const avgMbPerFile = document.getElementById('avg-mb-file');
animateNumber(data?.avgMbPerFile ?? 0, avgMbPerFile, 1500);

// Pie Charts

/* ... */

// VS

const emptyLinesVs = document.getElementById('empty-lines-vs');
animateNumber(data?.emptyLinesVs ?? 0, emptyLinesVs, 2000, 0);

const linesOfCodeVs = document.getElementById('lines-code-vs');
animateNumber(100 - (data?.emptyLinesVs ?? 0), linesOfCodeVs, 2000, 0);

const whitespaceVs = document.getElementById('whitespace-vs');
animateNumber(data?.whitespaceVs ?? 0, whitespaceVs, 2000, 0);

const codeCharactersVs = document.getElementById('code-chars-vs');
animateNumber(100 - (data?.whitespaceVs ?? 0), codeCharactersVs, 2000, 0);

// Records

const largestByMb = document.getElementById('largest-by-mb');
animateNumber(data?.largestByMb ?? 0, largestByMb, 2000, 0, 1);
document.getElementById('largest-by-mb-file').textContent = data?.largestByMbFile ?? "~";

const largestByChars = document.getElementById('largest-by-chars');
animateNumber(data?.largestByChars ?? 0, largestByChars, 2000, 0);
document.getElementById('largest-by-chars-file').textContent = data?.largestByCharsFile ?? "~";

const largestByLines = document.getElementById('largest-by-lines');
animateNumber(data?.largestByLines ?? 0, largestByLines, 2000, 0);
document.getElementById('largest-by-lines-file').textContent = data?.largestByLinesFile ?? "~";

const highestDensity = document.getElementById('highest-density');
animateNumber(data?.highestDensity ?? 0, highestDensity, 2000, 0, 1);
document.getElementById('highest-density-file').textContent = data?.highestDesnityFile ?? "~";

const oldestFileDays = document.getElementById('oldest-file-days');
animateNumber(data?.oldestFileDays ?? 0, oldestFileDays, 2000, 0);
document.getElementById('oldest-file').textContent = data?.oldestFile ?? "~";

const newestFileDays = document.getElementById('newest-file-days');
animateNumber(data?.newestFileDays ?? 0, newestFileDays, 2000, 0);
document.getElementById('newest-file').textContent = data?.newestFile ?? "~";