from pydantic import BaseModel, EmailStr
from datetime import datetime
from typing import Optional

from pydantic.types import conint


class Prostate(BaseModel):
    radius: int
    texture: int
    perimeter: int
    area: int
    smoothness: float
    compactness: float
    symmetry: float
    fractal_dimension: float
