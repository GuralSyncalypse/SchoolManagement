import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatScore',
  standalone: true
})
export class FormatScorePipe implements PipeTransform {
  transform(value: number | null | undefined): string {
    if (value === null || value === undefined) {
      return 'N/A';
    }
    return value.toLocaleString('vi-VN', { minimumFractionDigits: 1, maximumFractionDigits: 2 });
  }
}
