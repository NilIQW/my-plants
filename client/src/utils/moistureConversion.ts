export function convertRawToPercent(raw: number, dry = 2500, wet = 1000): number {
    const clamped = Math.max(wet, Math.min(raw, dry));
    return Math.round(100 - ((clamped - wet) * 100) / (dry - wet));
}

export function convertPercentToRaw(percent: number, dry = 2500, wet = 1000): number {
    const clamped = Math.max(0, Math.min(percent, 100));
    return Math.round(wet + ((100 - clamped) * (dry - wet)) / 100);
}
