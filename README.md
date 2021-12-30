# GoTools: C# and TypeScript tools
This project provides some useful tools for accelerating interoperability between C# and TypeScript code.

Features implemented include:
- Language-agnostic keyword/identifier tokenizer
- Recursive-descent parser for C# and TypeScript model classes
- C# and TypeScript model generation

## Automatically converting between C# and TypeScript models
The program provides a way for automatically converting between C# and TypeScript model classes. Take this C# model for example:
```csharp
using System;

namespace GoTools.ExampleModels;

public record UserCreateModel
{
    public string Name { get; init; }
    public DateTime? BirthDate { get; init; }
    public string? UserName { get; init; }
    public CountryModel[] Countries { get; init; }
    
    public record CountryModel
    {
        public Guid CountryId { get; init; }
        public string? CountryName { get; init; }
    }
}
```
The tool will then convert this into the following TypeScript model:
```typescript
export class UserCreateModel {
    public name: string;
    public birthDate: Date | null;
    public userName: string | null;
    public countries: UserCreateModel.CountryModel[];
}

export namespace UserCreateModel {
    export class CountryModel {
        public countryId: string;
        public countryName: string | null;
    }
}
```
## Synchronizing out-of-sync models (to do)
By considering each of the models within a project, differences between models can be automatically generated to ensure both models remain in sync.
Static analysis of each of the respective model definitions takes place to ensure that type and member information is consistent across both models.
